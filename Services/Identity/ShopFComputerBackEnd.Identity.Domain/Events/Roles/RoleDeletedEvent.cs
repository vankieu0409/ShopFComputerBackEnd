using Iot.Core.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Identity.Domain.Events.Roles
{
    public class RoleDeletedEvent:EventBase
    {
        public RoleDeletedEvent(Guid id, Guid? deletedBy)
        {
            Id = id;
            DeletedBy = deletedBy;
            IsDeleted = true;
            DeletedTime = DateTime.UtcNow;
        }
        public Guid Id { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTimeOffset DeletedTime { get; set; }
    }
}
