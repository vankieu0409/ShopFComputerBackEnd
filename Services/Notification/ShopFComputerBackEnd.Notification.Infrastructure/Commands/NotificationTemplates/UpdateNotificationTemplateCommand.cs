using ShopFComputerBackEnd.Notification.Domain.Dtos;
using ShopFComputerBackEnd.Notification.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Notification.Infrastructure.Commands.NotificationTemplates
{
    public class UpdateNotificationTemplateCommand : IRequest<NotificationDto>
    {
        public UpdateNotificationTemplateCommand(Guid id, NotificationTemplateEntity template)
        {
            Id = id;
            Template = template;
        }
        public Guid Id { get; set; }
        public NotificationTemplateEntity Template { get; set; }
        public Guid? ModifiedBy { get; set; }
    }
}
