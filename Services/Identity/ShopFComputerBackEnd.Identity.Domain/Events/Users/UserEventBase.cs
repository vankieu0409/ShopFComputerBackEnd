using System;

namespace ShopFComputerBackEnd.Identity.Domain.Events.Users
{
    public class UserEventBase
    {
        internal UserEventBase(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; private set; }
    }
}
