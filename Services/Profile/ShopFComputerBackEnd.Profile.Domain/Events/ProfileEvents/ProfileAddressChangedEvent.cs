using Iot.Core.Domain.Events;
using ShopFComputerBackEnd.Profile.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace ShopFComputerBackEnd.Profile.Domain.Events
{
    public class ProfileAddressChangedEvent: EventBase
    {
        public ProfileAddressChangedEvent(Guid id, List<AddressValueObject> address, Guid? modifiedBy)
        {
            Id = id;
            Address = address;
            ModifiedBy = modifiedBy;
            ModifiedTime = DateTime.UtcNow;
        }
        public Guid Id { get; set; }
        public List<AddressValueObject> Address { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTimeOffset ModifiedTime { get; set; }
    }
}
