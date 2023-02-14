using ShopFComputerBackEnd.Notification.Domain.Dtos;
using MediatR;
using System;

namespace ShopFComputerBackEnd.Notification.Infrastructure.Commands.NotificationTemplates
{
    public class RemoveNotificationTemplateCommand : IRequest<NotificationDto>
    {
        public RemoveNotificationTemplateCommand(Guid notificationId, string languageCode)
        {
            NotificationId = notificationId;
            LanguageCode = languageCode;
        }
        public Guid NotificationId { get; set; }
        public string LanguageCode { get; set; }
        public Guid? ModifiedBy { get; set; }
    }
}
