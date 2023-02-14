using AutoMapper;
using Iot.Core.DependencyInjection.Abstraction.Interfaces;
using Iot.Core.EventBus.Base.Abstractions;
using ShopFComputerBackEnd.Notification.Data.Repositories.Interfaces;
using ShopFComputerBackEnd.Notification.Domain.Enums;
using ShopFComputerBackEnd.Notification.Domain.Models;
using ShopFComputerBackEnd.Notification.Domain.ReadModels;
using ShopFComputerBackEnd.Notification.Domain.ValueObjects;
using ShopFComputerBackEnd.Notification.Infrastructure.Commands.Histories;
using ShopFComputerBackEnd.Notification.Infrastructure.Queries.Notifications;
using ShopFComputerBackEnd.Notification.Shared.IntegrationEvents;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ShopFComputerBackEnd.Notification.Infrastructure.Handlers.IntegrationEvents
{
    public class NotificationIntegrationEventHandler : IIntegrationEventHandler<NotificationIntegrationEvent>, ITransientDependency
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IDeviceRepository _deviceRepository;
        private readonly INotificationTemplateRepository _notificationTemplateRepository;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        public NotificationIntegrationEventHandler(IMapper mapper, IMediator mediator,
            IDeviceRepository deviceRepository, INotificationTemplateRepository notificationTemplate,
            IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _deviceRepository = deviceRepository ?? throw new ArgumentNullException(nameof(deviceRepository));
            _notificationTemplateRepository = notificationTemplate ?? throw new ArgumentNullException(nameof(notificationTemplate));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task Handle(NotificationIntegrationEvent @event)
        {
            var getDevice = _deviceRepository.AsQueryable().Where(entity => @event.ProfileIds.Contains(entity.ProfileId)).Select(device => device.Devicetoken).ToList();

            var getNotification = new GetNotificationDetailByContextAndNameAndTypeQuery(@event.Context, @event.Name, (NotificationType)@event.Type);
            var resultNotification = await _mediator.Send(getNotification);

            var templateQuery = await _notificationTemplateRepository.AsQueryable().FirstOrDefaultAsync(entity => Guid.Equals(entity.NotificationId, resultNotification.Id));

            await SendNotification(@event, getDevice, templateQuery);

        }

        private string ReplaceKeyInString(Dictionary<string, string> dictionary, string inputString)
        {
            var regex = new Regex("{(.*?)}");
            var matches = regex.Matches(inputString);
            foreach (Match match in matches)
            {
                var valueWithoutBrackets = match.Groups[1].Value;
                var valueWithBrackets = match.Value;

                if (dictionary.ContainsKey(valueWithoutBrackets))
                    inputString = inputString.Replace(valueWithBrackets, dictionary[valueWithoutBrackets]);
            }

            return inputString;
        }

        public async Task SendNotification(NotificationIntegrationEvent @event, List<string> getDevice, NotificationTemplateReadModel templateQuery)
        {
            var url = $"https://fcm.googleapis.com/fcm/send";
            var title = ReplaceKeyInString(@event.Variables, templateQuery.Subject);
            var body = ReplaceKeyInString(@event.Variables, templateQuery.Content);

            var postData = new FirebaseFcmModel()
            {
                RegistrationIds = getDevice,
                Payload = @event.Payload,
                Notification = new FcmNotification()
                {
                    Title = title,
                    Content = body,
                    Body = body,
                    Data = @event.Data
                }
            };

            var postJsonData = JsonSerializer.Serialize(postData);

            var postJson = new StringContent(postJsonData, Encoding.UTF8, Application.Json);

            var key = _configuration.GetValue<string>("Notification:Key");

            var isSuccess = NotificationStatus.Success;
            var message = string.Empty;

            try
            {
                using (var httpClient = _httpClientFactory.CreateClient())
                {
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation(HeaderNames.Authorization, key);
                    using var httpResponseMessage = await httpClient.PostAsync(url, postJson);
                    httpResponseMessage.EnsureSuccessStatusCode();
                    message = await httpResponseMessage.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                isSuccess = NotificationStatus.Fail;
                message = ex.Message;
            }

            var notificationValues = new NotificationBuiltValueObject()
            {
                Context = @event.Context,
                Name = @event.Name,
                LanguageCode = "vi-VN",
                Variables = @event.Variables.Select(value => new NotificationVariableValueObject()
                {
                    Variable = value.Key,
                    Description = value.Value
                })
            };
            var historyId = Guid.NewGuid();
            foreach (var entity in @event.ProfileIds)
            {
                var createHistoryCommand = new CreateHistoryCommand(historyId, templateQuery.Id, postJsonData, (NotificationType)@event.Type, _configuration.GetValue<string>("Notification:Key"), isSuccess, message, entity.ToString(), notificationValues);
                await _mediator.Send(createHistoryCommand);
            }


        }
    }
}
