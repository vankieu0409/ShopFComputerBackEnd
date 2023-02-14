using Iot.Core.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Identity.Domain.Events.Roles
{
    public class RoleRecoveredEvent: EventBase
    {
        public RoleRecoveredEvent(Guid id, Guid? modifiedBy)
        {
            Id = id;
            ModifiedBy = modifiedBy;
            IsDeleted = false;
            ModifiedTime = DateTime.UtcNow;
        }
        public Guid Id { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTimeOffset ModifiedTime { get; set; }
    }
}
