using Iot.Core.Domain.Entities;
using ShopFComputerBackEnd.Notification.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace ShopFComputerBackEnd.Notification.Domain.ReadModels
{
    public class NotificationTemplateReadModel : Entity<Guid>
    {
        public NotificationTemplateReadModel(Guid id, string languageCode, string subject, string content, IEnumerable<NotificationTemplateAttachmentValueObject> attachments, Guid notificationId)
        {
            Id = id;
            LanguageCode = languageCode;
            Subject = subject;
            Content = content;
            Attachments = attachments;
            NotificationId = notificationId;
        }
        public string LanguageCode { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public IEnumerable<NotificationTemplateAttachmentValueObject> Attachments { get; set; }
        public Guid NotificationId { get; set; }
        public NotificationReadModel Notification { get; set; }
        public IEnumerable<HistoryReadModel> Histories { get; set; }
    }
}
