using ShopFComputerBackEnd.Notification.Domain.Dtos;
using MediatR;
using System;

namespace ShopFComputerBackEnd.Notification.Infrastructure.Queries.NotificationTemplates
{
    public class GetNotificationTemplateDetailByLanguageCodeAndNotificationIdQuery : IRequest<NotificationTemplateDto>
    {
        public GetNotificationTemplateDetailByLanguageCodeAndNotificationIdQuery(string languageCode, Guid notificationId)
        {
            LanguageCode = languageCode;
            NotificationId = notificationId;
        }
        public string LanguageCode { get; set; }
        public Guid NotificationId { get; set; }
    }
}
