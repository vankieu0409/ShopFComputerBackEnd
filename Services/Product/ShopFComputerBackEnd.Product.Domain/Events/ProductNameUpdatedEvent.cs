using Iot.Core.Domain.Events;
using System;

namespace ShopFComputerBackEnd.Product.Domain.Events
{
    public class ProductNameUpdatedEvent : EventBase
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid ModefiledBy { get; set; }
        public DateTimeOffset ModefiledTime { get; set; }
        public ProductNameUpdatedEvent(Guid id, string name, Guid modefiledBy)
        {
            Id = id;
            Name = name;
            ModefiledBy = modefiledBy;
            ModefiledTime = DateTimeOffset.UtcNow;
        }
    }
}
