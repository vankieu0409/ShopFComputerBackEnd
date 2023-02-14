using Iot.Core.Domain.Events;
using ShopFComputerBackEnd.Notification.Domain.Entities;
using System;

namespace ShopFComputerBackEnd.Notification.Domain.Events.NotificationTemplates
{
    public class NotificationTemplateRemovedEvent : EventBase
    {
        public NotificationTemplateRemovedEvent(Guid id, NotificationTemplateEntity template, Guid? modifiedBy)
        {
            Id = id;
            Template = template;
            ModifiedBy = modifiedBy;
            ModifiedTime = DateTimeOffset.UtcNow;
        }
        public Guid Id { get; private set; }
        public NotificationTemplateEntity Template { get; private set; }
        public Guid? ModifiedBy { get; private set; }
        public DateTimeOffset ModifiedTime { get; private set; }
    }
}
