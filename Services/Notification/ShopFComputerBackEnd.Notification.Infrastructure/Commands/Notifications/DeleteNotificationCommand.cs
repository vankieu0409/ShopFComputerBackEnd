using ShopFComputerBackEnd.Notification.Domain.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Notification.Infrastructure.Commands.Notifications
{
    public class DeleteNotificationCommand : IRequest<NotificationDto>
    {
        public DeleteNotificationCommand(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
        public Guid? DeletedBy { get; set; }
    }
}
