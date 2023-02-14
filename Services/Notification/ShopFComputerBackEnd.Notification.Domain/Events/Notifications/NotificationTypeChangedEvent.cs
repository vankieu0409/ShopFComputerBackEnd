using Iot.Core.Domain.Events;
using ShopFComputerBackEnd.Notification.Domain.Enums;
using System;

namespace ShopFComputerBackEnd.Notification.Domain.Events
{
    public class NotificationTypeChangedEvent : EventBase
    {
        public NotificationTypeChangedEvent(Guid id, NotificationType type, Guid? modifiedBy)
        {
            Id = id;
            Type = type;
            ModifiedBy = modifiedBy;
            ModifiedTime = DateTimeOffset.UtcNow;
        }
        public Guid Id { get; private set; }
        public NotificationType Type { get; private set; }
        public Guid? ModifiedBy { get; private set; }
        public DateTimeOffset ModifiedTime { get; private set; }
    }
}
