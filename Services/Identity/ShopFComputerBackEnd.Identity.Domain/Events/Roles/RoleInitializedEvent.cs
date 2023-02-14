using Iot.Core.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Identity.Domain.Events.Roles
{
    public class RoleInitializedEvent : EventBase
    {
        public RoleInitializedEvent(Guid id, string name, Guid? createdBy )
        {
            Id = id;
            Name = name;
            CreateTime = DateTime.UtcNow;
            CreatedBy = createdBy;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset CreateTime  { get; set; }
        public Guid? CreatedBy { get; set; }
    }
}
