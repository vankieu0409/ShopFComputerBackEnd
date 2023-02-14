using Iot.Core.Domain.Events;
using System;

namespace ShopFComputerBackEnd.Profile.Domain.Events
{
    public class ProfileDisplayNameChangedEvent : EventBase
    {
        public ProfileDisplayNameChangedEvent(Guid id, string displayName, Guid? modifiedBy)
        {
            Id = id;
            DisplayName = displayName;
            ModifiedBy = modifiedBy;
            ModifiedTime = DateTimeOffset.UtcNow;
        }
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTimeOffset ModifiedTime { get; set; }
    }
}
