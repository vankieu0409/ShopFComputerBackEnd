using Iot.Core.Domain.Entities;
using System;
using System.Collections.Generic;

namespace ShopFComputerBackEnd.Notification.Domain.ValueObjects
{
    public class DeviceValueObject : Entity
    {
        public string Devicetoken { get; set; }
        public Guid UserId { get; set; }
        public ICollection<Guid> ProfileId { get; set; }
    }
}
