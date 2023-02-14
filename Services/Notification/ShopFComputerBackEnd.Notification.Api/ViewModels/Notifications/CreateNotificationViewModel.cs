using ShopFComputerBackEnd.Notification.Domain.Enums;
using ShopFComputerBackEnd.Notification.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Notification.Api.ViewModels.Notifications
{
    public class CreateNotificationViewModel
    {
        public string Context { get; set; }
        public string Name { get; set; }
        public ICollection<NotificationVariableValueObject> Variables { get; set; }
        public NotificationType Type { get; set; }
    }
}
