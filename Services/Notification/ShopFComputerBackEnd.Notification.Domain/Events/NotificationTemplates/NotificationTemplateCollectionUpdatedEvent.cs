using Iot.Core.Domain.Events;
using ShopFComputerBackEnd.Notification.Domain.Entities;
using System;
using System.Collections.Generic;

namespace ShopFComputerBackEnd.Notification.Domain.Events.NotificationTemplates
{
    public class NotificationTemplateCollectionUpdatedEvent : EventBase
    {
        public NotificationTemplateCollectionUpdatedEvent(Guid id, ICollection<NotificationTemplateEntity> templates, Guid? modifiedBy)
        {
            Id = id;
            Templates = templates;
            ModifiedBy = modifiedBy;
            ModifiedTime = DateTimeOffset.UtcNow;
        }
        public Guid Id { get; private set; }
        public ICollection<NotificationTemplateEntity> Templates { get; private set; }
        public DateTimeOffset ModifiedTime { get; private set; }
        public Guid? ModifiedBy { get; private set; }
    }
}
