using Iot.Core.Domain.Implements;
using ShopFComputerBackEnd.Notification.Domain.Enums;
using ShopFComputerBackEnd.Notification.Domain.ValueObjects;
using System;

namespace ShopFComputerBackEnd.Notification.Domain.Dtos
{
    public class HistoryDto : FullAuditedEntity<Guid>
    {
        public Guid TemplateId { get; set; }
        public string Content { get; set; }
        public string Action { get; set; }
        public string GenealogyName { get; set; }
        public DateTimeOffset ActionTime { get; set; }
        public NotificationType Type { get; set; }
        public string ConfigurationType { get; set; }
        public NotificationStatus Status { get; set; }
        public string Message { get; set; }
        public DateTimeOffset SentTime { get; set; }
        public string Destination { get; set; }
        public NotificationBuiltValueObject RawData { get; set; }
    }
}
