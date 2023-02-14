using Iot.Core.Domain.Implements;
using ShopFComputerBackEnd.Notification.Domain.Enums;
using ShopFComputerBackEnd.Notification.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace ShopFComputerBackEnd.Notification.Domain.ReadModels
{
    public class NotificationReadModel : FullEntity<Guid>
    {
        public string Context { get; set; }
        public string Name { get; set; }
        public ICollection<NotificationTemplateReadModel> Templates { get; set; }
        public ICollection<NotificationVariableValueObject> Variables { get; set; }
        public NotificationType Type { get; set; }
    }
}
