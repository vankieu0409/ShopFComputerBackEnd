using Iot.Core.Domain.Events;
using System;

namespace ShopFComputerBackEnd.Profile.Domain.Events
{
    public class ProfileDeletedEvent : EventBase
    {
        public ProfileDeletedEvent(Guid id, Guid? deletedBy)
        {
            Id = id;
            DeletedBy = deletedBy;
            DeletedTime = DateTime.UtcNow;
        }

        public Guid Id { get; private set; }
        public Guid? DeletedBy { get; private set; }
        public DateTimeOffset DeletedTime { get; private set; }
    }
}
