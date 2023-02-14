using Iot.Core.Domain.Entities;
using System;

namespace ShopFComputerBackEnd.Identity.Domain.ValueObjects
{
    public class DeviceValueObject : Entity
    {
        public string Devicetoken { get; set; }
        public Guid UserId { get; set; }
        public Guid Profileid { get; set; }
    }
}
