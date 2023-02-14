using AutoMapper;
using Iot.Core.Extensions;
using Iot.Core.Infrastructure.Exceptions;
using ShopFComputerBackEnd.Notification.Data.Repositories.Interfaces;
using ShopFComputerBackEnd.Notification.Domain.Events;
using ShopFComputerBackEnd.Notification.Domain.ReadModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Notification.Infrastructure.Handlers.Events
{
    public class NotificationEventHandler : INotificationHandler<NotificationInitializedEvent>,
                                            INotificationHandler<NotificationNameChangedEvent>,
                                            INotificationHandler<NotificationTypeChangedEvent>,
                                            INotificationHandler<NotificationDeletedEvent>,
                                            INotificationHandler<NotificationRecoveredEvent>,
                                            INotificationHandler<NotificationVariableCollectionUpdatedEvent>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<NotificationEventHandler> _logger;
        public NotificationEventHandler(INotificationRepository notificationRepository, IMapper mapper,
            ILogger<NotificationEventHandler> logger)
        {
            _notificationRepository = notificationRepository ?? throw new ArgumentNullException(nameof(notificationRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task Handle(NotificationInitializedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Event - {notification.Id} - Start Handle {nameof(notification)}");

            var mainNotification = _mapper.Map<NotificationReadModel>(notification);
            _logger.LogDebug($"Event - {notification.Id} - Handle {nameof(notification)} \n Payload: \n {JsonSerializer.Serialize(mainNotification)}");

            await _notificationRepository.AddAsync(mainNotification);
            await _notificationRepository.SaveChangesAsync();
            _logger.LogDebug($"Event - {notification.Id} - End Handle {nameof(mainNotification)}");
        }

        public async Task Handle(NotificationNameChangedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Event - {notification.Id} - Start Handle {nameof(notification)}");

            var mainNotification = await _notificationRepository.AsQueryable().FirstOrDefaultAsync(mainNotification => Guid.Equals(mainNotification.Id, notification.Id));
            mainNotification.Name = notification.Name;
            mainNotification.ModifiedBy = notification.ModifiedBy;
            _logger.LogDebug($"Event - {notification.Id} - Handle {nameof(notification)} \n Payload: \n {JsonSerializer.Serialize(mainNotification)}");

            await _notificationRepository.UpdateAsync(mainNotification);
            await _notificationRepository.SaveChangesAsync();
            _logger.LogDebug($"Event - {notification.Id} - End Handle {nameof(mainNotification)}");
        }

        public async Task Handle(NotificationTypeChangedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Event - {notification.Id} - Start Handle {nameof(notification)}");

            var mainNotification = await _notificationRepository.AsQueryable().FirstOrDefaultAsync(mainNotification => Guid.Equals(mainNotification.Id, notification.Id));
            mainNotification.Type = notification.Type;
            mainNotification.ModifiedBy = notification.ModifiedBy;
            _logger.LogDebug($"Event - {notification.Id} - Handle {nameof(notification)} \n Payload: \n {JsonSerializer.Serialize(mainNotification)}");

            await _notificationRepository.UpdateAsync(mainNotification);
            await _notificationRepository.SaveChangesAsync();
            _logger.LogDebug($"Event - {notification.Id} - End Handle {nameof(mainNotification)}");
        }

        public async Task Handle(NotificationDeletedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Event - {notification.Id} - Start Handle {nameof(notification)}");

            var mainNotification = await _notificationRepository.AsQueryable().FirstOrDefaultAsync(mainNotification => Guid.Equals(mainNotification.Id, notification.Id));
            if (mainNotification.IsNullOrDefault() || !mainNotification.IsDeleted)
                throw new EntityNotFoundException();
            mainNotification.IsDeleted = true;
            mainNotification.DeletedBy = notification.DeletedBy;
            _logger.LogDebug($"Event - {notification.Id} - Handle {nameof(notification)} \n Payload: \n {JsonSerializer.Serialize(mainNotification)}");

            await _notificationRepository.RemoveAsync(mainNotification);
            await _notificationRepository.SaveChangesAsync();
            _logger.LogDebug($"Event - {notification.Id} - End Handle {nameof(mainNotification)}");
        }

        public async Task Handle(NotificationRecoveredEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Event - {notification.Id} - Start Handle {nameof(notification)}");

            var mainNotification = await _notificationRepository.AsQueryable().FirstOrDefaultAsync(mainNotification => Guid.Equals(mainNotification.Id, notification.Id));
            if (mainNotification.IsNullOrDefault()||mainNotification.IsDeleted)
                throw new EntityAlreadyExistsException();
            mainNotification.IsDeleted = false;
            mainNotification.ModifiedBy = notification.ModifiedBy;
            _logger.LogDebug($"Event - {notification.Id} - Handle {nameof(notification)} \n Payload: \n {JsonSerializer.Serialize(mainNotification)}");

            await _notificationRepository.UpdateAsync(mainNotification);
            await _notificationRepository.SaveChangesAsync();
            _logger.LogDebug($"Event - {notification.Id} - End Handle {nameof(mainNotification)}");
        }

        public async Task Handle(NotificationVariableCollectionUpdatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Event - {notification.Id} - Start Handle {nameof(notification)}");

            var mainNotification = await _notificationRepository.AsQueryable().FirstOrDefaultAsync(mainNotification => Guid.Equals(mainNotification.Id, notification.Id));
            mainNotification.Variables = notification.Variables;
            mainNotification.ModifiedBy = notification.ModifiedBy;
            _logger.LogDebug($"Event - {notification.Id} - Handle {nameof(notification)} \n Payload: \n {JsonSerializer.Serialize(mainNotification)}");

            await _notificationRepository.UpdateAsync(mainNotification);
            await _notificationRepository.SaveChangesAsync();
            _logger.LogDebug($"Event - {notification.Id} - End Handle {nameof(mainNotification)}");
        }
    }
}
