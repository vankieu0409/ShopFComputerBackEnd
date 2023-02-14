using Iot.Core.Domain.Entities;
using ShopFComputerBackEnd.Notification.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace ShopFComputerBackEnd.Notification.Domain.Entities
{
    public class NotificationTemplateEntity : Entity<Guid>
    {
        public string LanguageCode { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public IEnumerable<NotificationTemplateAttachmentValueObject> Attachments { get; set; }
    }
}
