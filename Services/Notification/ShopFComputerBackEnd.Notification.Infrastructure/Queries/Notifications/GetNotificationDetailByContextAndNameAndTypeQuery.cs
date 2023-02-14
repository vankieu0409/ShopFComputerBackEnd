using ShopFComputerBackEnd.Notification.Domain.Dtos;
using ShopFComputerBackEnd.Notification.Domain.Enums;
using ShopFComputerBackEnd.Notification.Domain.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Notification.Infrastructure.Queries.Notifications
{
    public class GetNotificationDetailByContextAndNameAndTypeQuery : IRequest<NotificationDto>
    {
        public GetNotificationDetailByContextAndNameAndTypeQuery(string context, string name, NotificationType type)
        {
            Context = context;
            Name = name;
            Type = type;
        }
        public NotificationType Type { get; set; }
        public string Context { get; set; }
        public string Name { get; set; }
    }
}
