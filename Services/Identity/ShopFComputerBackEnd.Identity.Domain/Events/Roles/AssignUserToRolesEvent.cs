using Iot.Core.Domain.Events;
using System;

namespace ShopFComputerBackEnd.Identity.Domain.Events.Roles
{
    public class AssignUserToRolesEvent : EventBase
    {
        public AssignUserToRolesEvent(Guid id, string roleName)
        {
            Id = id;
            RoleName = roleName;
        }

        public Guid Id { get; set; }
        public string RoleName { get; set; }
    }
}
