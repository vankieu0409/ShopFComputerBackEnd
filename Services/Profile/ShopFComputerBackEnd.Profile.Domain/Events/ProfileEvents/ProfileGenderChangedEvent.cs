using Iot.Core.Domain.Events;
using ShopFComputerBackEnd.Profile.Domain.Enums;
using System;

namespace ShopFComputerBackEnd.Profile.Domain.Events
{
    public class ProfileGenderChangedEvent : EventBase
    {
        public ProfileGenderChangedEvent(Guid id, GendersType gender, Guid? modifiedBy)
        {
            Id = id;
            Gender = gender;
            ModifiedBy = modifiedBy;
            ModifiedTime = DateTime.UtcNow;
        }
        public Guid Id { get; set; }
        public GendersType Gender { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTimeOffset ModifiedTime { get; set; }
    }
}
