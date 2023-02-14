using AutoMapper;
using Iot.Core.EventStore.Abstraction.Interfaces;
using Iot.Core.Extensions;
using Iot.Core.Infrastructure.Exceptions;
using ShopFComputerBackEnd.Notification.Domain.Aggregates;
using ShopFComputerBackEnd.Notification.Domain.Dtos;
using ShopFComputerBackEnd.Notification.Infrastructure.Commands.NotificationTemplates;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Notification.Infrastructure.Handlers.Commands
{
    public class NotificationTemplateCommandHandler : IRequestHandler<UpdateNotificationTemplateCommand, NotificationDto>,
                                                        IRequestHandler<UpdateNotificationTemplateCollectionCommand, NotificationDto>,
                                                        IRequestHandler<RemoveNotificationTemplateCommand, NotificationDto>
    {
        private readonly IMapper _mapper;
        private readonly IEventStoreService<NotificationAggregateRoot> _eventStore;
        private readonly ILogger<NotificationTemplateCommandHandler> _logger;

        public NotificationTemplateCommandHandler(IMapper mapper, IEventStoreService<NotificationAggregateRoot> eventStore,
            ILogger<NotificationTemplateCommandHandler> logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<NotificationDto> Handle(UpdateNotificationTemplateCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Command - {request.Id} - Start Handle {nameof(request)}");
            var aggregateRoot = new NotificationAggregateRoot(request.Id);

            var aggregateStream = await _eventStore.AggregateStreamAsync(aggregateRoot.StreamName);
            if (aggregateStream.IsNullOrDefault())
                throw new EntityNotFoundException();

            _logger.LogDebug($"Command - {request.Id} - Handle {nameof(request)} \n Payload: \n {JsonSerializer.Serialize(aggregateRoot)}");
            aggregateStream.UpdateTemplate(request.Template, request.ModifiedBy);
            await _eventStore.AppendStreamAsync(aggregateStream.StreamName, aggregateStream);
            _logger.LogDebug($"Command - {request.Id} - End Handle {nameof(request)}");

            return _mapper.Map<NotificationDto>(aggregateStream);
        }
        public async Task<NotificationDto> Handle(UpdateNotificationTemplateCollectionCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Command - {request.Id} - Start Handle {nameof(request)}");
            var aggregateRoot = new NotificationAggregateRoot(request.Id);

            var notificaaggregateStream  = await _eventStore.AggregateStreamAsync(aggregateRoot.StreamName);
            if (notificaaggregateStream.IsNullOrDefault())
                throw new EntityNotFoundException();

            _logger.LogDebug($"Command - {request.Id} - Handle {nameof(request)} \n Payload: \n {JsonSerializer.Serialize(notificaaggregateStream)}");
            notificaaggregateStream.UpdateTemplateCollection(request.Templates, request.ModifiedBy);
            await _eventStore.AppendStreamAsync(notificaaggregateStream.StreamName, notificaaggregateStream);
            _logger.LogDebug($"Command - {request.Id} - End Handle {nameof(request)}");

            return _mapper.Map<NotificationDto>(notificaaggregateStream);
        }

        public async Task<NotificationDto> Handle(RemoveNotificationTemplateCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Command - {request.NotificationId} - Start Handle {nameof(request)}");
            var aggregateRoot = new NotificationAggregateRoot(request.NotificationId);

            var aggregateStream = await _eventStore.AggregateStreamAsync(aggregateRoot.StreamName);
            if (aggregateStream.IsNullOrDefault())
                throw new EntityNotFoundException();

            _logger.LogDebug($"Command - {request.NotificationId} - Handle {nameof(request)} \n Payload: \n {JsonSerializer.Serialize(aggregateRoot)}");
            aggregateStream.RemoveTemplate(request.LanguageCode, request.ModifiedBy);
            await _eventStore.AppendStreamAsync(aggregateStream.StreamName, aggregateStream);
            _logger.LogDebug($"Command - {request.NotificationId} - End Handle {nameof(request)}");

            return _mapper.Map<NotificationDto>(aggregateStream);
        }
    }
}
