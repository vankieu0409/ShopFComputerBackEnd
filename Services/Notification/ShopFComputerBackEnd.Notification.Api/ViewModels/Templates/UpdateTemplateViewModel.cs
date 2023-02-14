using ShopFComputerBackEnd.Notification.Domain.ReadModels;
using ShopFComputerBackEnd.Notification.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Notification.Api.ViewModels.Templates
{
    public class UpdateTemplateViewModel
    {
        public string LanguageCode { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public IEnumerable<NotificationTemplateAttachmentValueObject> Attachments { get; set; }
    }
}
