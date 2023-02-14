using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Notification.Domain.ValueObjects
{
    public class NotificationTemplateAttachmentValueObject
    {
        public string FileName { get; set; }
        public string Base64Content { get; set; }
        public string ContentType { get; set; }
    }
}
