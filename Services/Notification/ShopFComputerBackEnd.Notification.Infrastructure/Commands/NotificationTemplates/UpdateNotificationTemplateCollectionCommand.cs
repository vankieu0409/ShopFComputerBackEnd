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
    public class UpdateNotificationTemplateCollectionCommand : IRequest<NotificationDto>
    {
        public UpdateNotificationTemplateCollectionCommand(Guid id, ICollection<NotificationTemplateEntity> templates)
        {
            Id = id;
            Templates = templates;
        }
        public Guid Id { get; set; }
        public ICollection<NotificationTemplateEntity> Templates { get; set; }
        public Guid? ModifiedBy { get; set; }
    }
}
