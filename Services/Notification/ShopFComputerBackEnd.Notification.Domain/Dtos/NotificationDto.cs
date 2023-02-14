using Iot.Core.Domain.Implements;
using ShopFComputerBackEnd.Notification.Domain.Entities;
using ShopFComputerBackEnd.Notification.Domain.Enums;
using ShopFComputerBackEnd.Notification.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Notification.Domain.Dtos
{
    public class NotificationDto : FullAuditedEntity<Guid>
    {
        public string Context { get; set; }
        public string Name { get; set; }
        public NotificationTemplateEntity Template { get; set; }
        public ICollection<NotificationVariableValueObject> Variables { get; set; }
        public NotificationType Type { get; set; }
    }
}
