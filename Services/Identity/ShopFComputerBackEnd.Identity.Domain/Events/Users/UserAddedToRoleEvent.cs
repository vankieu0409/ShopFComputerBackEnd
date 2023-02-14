using Iot.Core.Domain.Events;
using System;
using System.Collections.Generic;

namespace ShopFComputerBackEnd.Identity.Domain.Events.Users
{
    public class UserAddedToRoleEvent : EventBase
    {
        public UserAddedToRoleEvent(Guid id, ICollection<string> roleName)
        {
            Id = id;
            RoleName = roleName;
        }
        public Guid Id { get; set; }
        public ICollection<string> RoleName { get; set; }
    }
}
