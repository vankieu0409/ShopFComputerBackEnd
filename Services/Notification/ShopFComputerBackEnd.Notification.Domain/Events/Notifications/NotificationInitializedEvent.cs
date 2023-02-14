using Iot.Core.Domain.Events;
using ShopFComputerBackEnd.Notification.Domain.Entities;
using ShopFComputerBackEnd.Notification.Domain.Enums;
using ShopFComputerBackEnd.Notification.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace ShopFComputerBackEnd.Notification.Domain.Events
{
    public class NotificationInitializedEvent : EventBase
    {
        public NotificationInitializedEvent(Guid id, string context, string name, NotificationTemplateEntity template, ICollection<NotificationVariableValueObject> variables, NotificationType type, Guid? createdBy)
        {
            Id = id;
            Context = context;
            Name = name;
            Template = template;
            Variables = variables;
            Type = type;
            CreatedBy = createdBy;
            CreatedTime = DateTimeOffset.UtcNow;
        }
        public Guid Id { get; private set; }
        public string Context { get; private set; }
        public string Name { get; private set; }
        public NotificationTemplateEntity Template { get; private set; }
        public ICollection<NotificationVariableValueObject> Variables { get; private set; }
        public NotificationType Type { get; private set; }
        public DateTimeOffset CreatedTime { get; private set; }
        public Guid? CreatedBy { get; private set; }
    }
}
