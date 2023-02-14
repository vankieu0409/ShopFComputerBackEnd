using Iot.Core.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Identity.Domain.Events.Roles
{
    public class RoleNameChangedEvent : EventBase
    {
        public RoleNameChangedEvent(Guid id, string name, Guid? modifiedBy)
        {
            Id = id;
            Name = name;
            ModifiedBy = modifiedBy;
            ModifiedTime = DateTime.UtcNow;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTimeOffset ModifiedTime { get; set; }
    }
}
