using ShopFComputerBackEnd.Notification.Domain.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Notification.Infrastructure.Queries.NotificationTemplates
{
    public class GetNotificationTemplateCollectionByNotificationIdQuery : IRequest<IQueryable<NotificationTemplateDto>>
    {
        public GetNotificationTemplateCollectionByNotificationIdQuery(Guid notificationId)
        {
            NotificationId = notificationId;
        }
        public Guid NotificationId { get; set; }
    }
}
