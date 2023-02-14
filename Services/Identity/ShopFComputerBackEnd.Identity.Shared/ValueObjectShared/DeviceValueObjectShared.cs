using System;

namespace ShopFComputerBackEnd.Identity.Domain.shared.ValueObjectShared
{
    public class DeviceValueObjectShared
    {
        public string Devicetoken { get; set; }
        public Guid UserId { get; set; }
        public Guid Profileid { get; set; }
    }
}
