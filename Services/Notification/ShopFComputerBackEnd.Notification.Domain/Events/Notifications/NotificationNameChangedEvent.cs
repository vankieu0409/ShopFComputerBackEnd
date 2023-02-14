using Iot.Core.Domain.Events;
using System;

namespace ShopFComputerBackEnd.Notification.Domain.Events
{
    public class NotificationNameChangedEvent : EventBase
    {
        public NotificationNameChangedEvent(Guid id, string name, Guid? modifiedBy)
        {
            Id = id;
            Name = name;
            ModifiedBy = modifiedBy;
            ModifiedTime = DateTimeOffset.UtcNow;
        }
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public Guid? ModifiedBy { get; private set; }
        public DateTimeOffset ModifiedTime { get; private set; }
    }
}
