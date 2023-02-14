using AutoMapper;
using ShopFComputerBackEnd.Notification.Data.Repositories.Interfaces;
using ShopFComputerBackEnd.Notification.Domain.Events.NotificationTemplates;
using ShopFComputerBackEnd.Notification.Domain.ReadModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Notification.Infrastructure.Handlers.Events
{
    public class NotificationTemplateEventHandler : INotificationHandler<NotificationTemplateUpdatedEvent>,
                                                    INotificationHandler<NotificationTemplateCollectionUpdatedEvent>,
                                                    INotificationHandler<NotificationTemplateRemovedEvent>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly INotificationTemplateRepository _notificationTemplateRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<NotificationTemplateEventHandler> _logger;
        public NotificationTemplateEventHandler(INotificationRepository notificationRepository, IMapper mapper,
            INotificationTemplateRepository notificationTemplateRepository, ILogger<NotificationTemplateEventHandler> logger)
        {
            _notificationRepository = notificationRepository ?? throw new ArgumentNullException(nameof(notificationRepository));
            _notificationTemplateRepository = notificationTemplateRepository ?? throw new ArgumentNullException(nameof(notificationTemplateRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task Handle(NotificationTemplateUpdatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Event - {notification.Id} - Start Handle {nameof(notification)}");

            var existTemplate = await _notificationTemplateRepository.AsQueryable().AnyAsync(template => Guid.Equals(template.NotificationId, notification.Id) && string.Equals(template.LanguageCode, notification.Template.LanguageCode));
            var template = new NotificationTemplateReadModel(notification.Template.Id,notification.Template.LanguageCode, notification.Template.Subject, notification.Template.Content, notification.Template.Attachments, notification.Id);
            _logger.LogDebug($"Event - {notification.Id} - Handle {nameof(notification)} \n Payload: \n {JsonSerializer.Serialize(existTemplate)}");

            if (!existTemplate)
                await _notificationTemplateRepository.AddAsync(template);
            else
                await _notificationTemplateRepository.UpdateAsync(template);
            await _notificationRepository.SaveChangesAsync();
            _logger.LogDebug($"Event - {notification.Id} - End Handle {nameof(existTemplate)}");
        }

        public async Task Handle(NotificationTemplateCollectionUpdatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Event - {notification.Id} - Start Handle {nameof(notification)}");

            var existTemplates = await _notificationTemplateRepository.AsQueryable().AsNoTracking().Where(template => Guid.Equals(template.NotificationId, notification.Id)).ToListAsync();

            var templateCollectionToAdd = notification.Templates
                .Where(template => !existTemplates.Any(entity => string.Equals(entity.LanguageCode, template.LanguageCode)))
                .Select(template => new NotificationTemplateReadModel(template.Id, template.LanguageCode, template.Subject, template.Content, template.Attachments, notification.Id));
            _logger.LogDebug($"Event - {notification.Id} - Handle {nameof(notification)} \n Payload: \n {JsonSerializer.Serialize(templateCollectionToAdd)}");

            var templateCollectionToUpdate = notification.Templates
                .Where(template => existTemplates.Any(entity => string.Equals(entity.LanguageCode, template.LanguageCode)))
                .Select(template => new NotificationTemplateReadModel(template.Id, template.LanguageCode, template.Subject, template.Content, template.Attachments, notification.Id));
            _logger.LogDebug($"Event - {notification.Id} - Handle {nameof(notification)} \n Payload: \n {JsonSerializer.Serialize(templateCollectionToAdd)}");

            var templateCollectionToRemove = existTemplates
                .Where(template => !notification.Templates.Any(entity => string.Equals(entity.LanguageCode, template.LanguageCode)));
            _logger.LogDebug($"Event - {notification.Id} - Handle {nameof(notification)} \n Payload: \n {JsonSerializer.Serialize(templateCollectionToAdd)}");

            if (templateCollectionToRemove.Any())
                await _notificationTemplateRepository.RemoveRangeAsync(templateCollectionToRemove);

            if (templateCollectionToUpdate.Any())
                await _notificationTemplateRepository.UpdateRangeAsync(templateCollectionToUpdate);

            if (templateCollectionToRemove.Any())
                await _notificationTemplateRepository.AddRangeAsync(templateCollectionToAdd);

            await _notificationRepository.SaveChangesAsync();
            _logger.LogDebug($"Event - {notification.Id} - End Handle {nameof(templateCollectionToAdd)}");
        }

        public async Task Handle(NotificationTemplateRemovedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Event - {notification.Id} - Start Handle {nameof(notification)}");

            var existTemplate = await _notificationTemplateRepository.AsQueryable().FirstOrDefaultAsync(template => Guid.Equals(template.NotificationId, notification.Id) && string.Equals(template.LanguageCode, notification.Template.LanguageCode));
            _logger.LogDebug($"Event - {notification.Id} - Handle {nameof(notification)} \n Payload: \n {JsonSerializer.Serialize(existTemplate)}");

            await _notificationTemplateRepository.RemoveAsync(existTemplate);
            await _notificationRepository.SaveChangesAsync();
            _logger.LogDebug($"Event - {notification.Id} - End Handle {nameof(existTemplate)}");
        }
    }
}
