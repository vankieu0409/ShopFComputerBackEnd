using Iot.Core.Domain.Events;
using System;

namespace ShopFComputerBackEnd.Identity.Domain.Events.Roles
{
    public class AssignRoleToUserEvent : EventBase
    {
        public AssignRoleToUserEvent(Guid id, string roleName)
        {
            Id = id;
            RoleName = roleName;
        }

        public Guid Id { get; set; }
        public string RoleName { get; set; }
    }
}
