using ShopFComputerBackEnd.Notification.Domain.Dtos;
using ShopFComputerBackEnd.Notification.Domain.Entities;
using ShopFComputerBackEnd.Notification.Domain.Enums;
using ShopFComputerBackEnd.Notification.Domain.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Notification.Infrastructure.Commands.Notifications
{
    public class UpdateNotificationCommand : IRequest<NotificationDto>
    {
        public UpdateNotificationCommand(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
        public string Context { get; set; }
        public string Name { get; set; }
        public ICollection<NotificationVariableValueObject> Variables { get; set; }
        public NotificationType Type { get; set; }
        public Guid ModifiedBy { get; set; }

    }
}
