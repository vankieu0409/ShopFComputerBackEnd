using Iot.Core.Domain.Entities;
using ShopFComputerBackEnd.Notification.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace ShopFComputerBackEnd.Notification.Domain.Dtos
{
    public class NotificationTemplateDto : Entity
    {
        public Guid Id { get; set; }
        public string LanguageCode { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public IEnumerable<NotificationTemplateAttachmentValueObject> Attachments { get; set; }
        public Guid NotificationId { get; set; }
    }
}
