using ShopFComputerBackEnd.Notification.Domain.Enums;
using ShopFComputerBackEnd.Notification.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ShopFComputerBackEnd.Notification.Domain.Enums.NotificationStatus;

namespace ShopFComputerBackEnd.Notification.Api.ViewModels
{
    public class CreateHistoryViewModel
    {
        public Guid TemplateId { get; set; }
        public string Content { get; set; }
        public NotificationType Type { get; set; }
        public string ConfigurationType { get; set; }
        public NotificationStatus Status { get; set; }
        public string Message { get; set; }
        public DateTimeOffset SentTime { get; set; }
        public string Destination { get; set; }
        public NotificationBuiltValueObject RawData { get; set; }
    }
}
