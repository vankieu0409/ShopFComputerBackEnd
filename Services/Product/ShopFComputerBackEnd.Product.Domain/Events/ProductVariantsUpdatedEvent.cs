using Iot.Core.Domain.Events;
using ShopFComputerBackEnd.Product.Domain.Entities;
using System;
using System.Collections.Generic;

namespace ShopFComputerBackEnd.Product.Domain.Events
{
    public class ProductVariantsUpdatedEvent : EventBase
    {
        public Guid Id { get; set; }
        public Guid ProductVariantId { get; set; }
        public List<OptionValueEntity> Options { get; set; }
        public Guid ModefiledBy { get; set; }
        public DateTimeOffset ModefiledTime { get; set; }

        public ProductVariantsUpdatedEvent(Guid id, Guid productVariantId, List<OptionValueEntity> options, Guid modefiledBy)
        {
            Id = id;
            ProductVariantId = productVariantId;
            Options = options;
            ModefiledBy = modefiledBy;
            ModefiledTime = DateTimeOffset.UtcNow;
        }
    }
}
