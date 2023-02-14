using Iot.Core.Domain.Events;
using System;

namespace ShopFComputerBackEnd.Notification.Domain.Events
{
    public class NotificationRecoveredEvent : EventBase
    {
        public NotificationRecoveredEvent(Guid id, Guid? modifiedBy)
        {
            Id = id;
            IsDeleted = false;
            ModifiedBy = modifiedBy;
            ModifiedTime = DateTimeOffset.UtcNow;
        }
        public Guid Id { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTimeOffset ModifiedTime { get; set; }
    }
}
