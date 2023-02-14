using Iot.Core.Domain.Events;
using ShopFComputerBackEnd.Notification.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace ShopFComputerBackEnd.Notification.Domain.Events
{
    public class NotificationVariableCollectionUpdatedEvent : EventBase
    {
        public NotificationVariableCollectionUpdatedEvent(Guid id, ICollection<NotificationVariableValueObject> variables, Guid? modifiedBy)
        {
            Id = id;
            Variables = variables;
            ModifiedBy = modifiedBy;
            ModifiedTime = DateTimeOffset.UtcNow;
        }
        public Guid Id { get; private set; }
        public ICollection<NotificationVariableValueObject> Variables { get; private set; }
        public Guid? ModifiedBy { get; private set; }
        public DateTimeOffset ModifiedTime { get; private set; }
    }
}
