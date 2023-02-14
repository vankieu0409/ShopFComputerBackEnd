using Iot.Core.Domain.Events;
using ShopFComputerBackEnd.Product.Domain.Entities;
using System;
using System.Collections.Generic;

namespace ShopFComputerBackEnd.Product.Domain.Events
{
    public class ProductOptionUpdatedEvent : EventBase
    {
        public Guid Id { get; set; }
        public List<OptionEntity> Options { get; set; }
        public Guid ModefiledBy { get; set; }
        public DateTimeOffset ModefiledTime { get; set; }

        public ProductOptionUpdatedEvent(Guid id, List<OptionEntity> options, Guid modefiledBy)
        {
            Id = id;
            Options = options;
            ModefiledBy = modefiledBy;
            ModefiledTime = DateTimeOffset.UtcNow;
        }
    }
}
