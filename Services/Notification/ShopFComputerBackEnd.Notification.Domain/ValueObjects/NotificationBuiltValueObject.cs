using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Notification.Domain.ValueObjects
{
    public class NotificationBuiltValueObject
    {
        public string Context { get; set; }
        public string Name { get; set; }
        public IEnumerable<NotificationVariableValueObject> Variables { get; set; }
        public string LanguageCode { get; set; }
    }
}
