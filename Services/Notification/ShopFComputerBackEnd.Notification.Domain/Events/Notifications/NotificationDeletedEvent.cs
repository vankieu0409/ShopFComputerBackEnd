using Iot.Core.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Notification.Domain.Events
{
    public class NotificationDeletedEvent : EventBase
    {
        public NotificationDeletedEvent(Guid id, Guid? deletedBy)
        {
            Id = id;
            IsDeleted = true;
            DeletedBy = deletedBy;
            DeletedTime = DateTimeOffset.UtcNow;
        }
        public Guid Id { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTimeOffset DeletedTime { get; set; }
    }
}
