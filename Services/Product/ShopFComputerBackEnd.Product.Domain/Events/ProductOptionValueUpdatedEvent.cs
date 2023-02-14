using Iot.Core.Domain.Events;
using ShopFComputerBackEnd.Product.Domain.Entities;
using System;
using System.Collections.Generic;

namespace ShopFComputerBackEnd.Product.Domain.Events
{
    public class ProductOptionValueUpdatedEvent : EventBase
    {
        public Guid Id { get; set; }
        public Guid ProductVariantOptionId { get; set; }
        public List<OptionValueEntity> Values { get; set; }
        public Guid ModefiledBy { get; set; }
        public DateTimeOffset ModefiledTime { get; set; }

        public ProductOptionValueUpdatedEvent(Guid id,Guid productVariantOptionId ,List<OptionValueEntity> optionValues, Guid modefiledBy)
        {
            Id = id;
            ProductVariantOptionId = productVariantOptionId;
            Values = optionValues;
            ModefiledBy = modefiledBy;
            ModefiledTime = DateTimeOffset.UtcNow;
        }
    }
}
