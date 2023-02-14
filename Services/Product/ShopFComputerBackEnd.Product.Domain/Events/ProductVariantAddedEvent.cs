using Iot.Core.Domain.Events;
using ShopFComputerBackEnd.Product.Domain.Entities;
using System;
using System.Collections.Generic;

namespace ShopFComputerBackEnd.Product.Domain.Events
{
    public class ProductVariantAddedEvent : EventBase
    {
        public Guid Id { get; set; }
        public List<ProductVariantEntity> ProductVariants { get; set; }
        public Guid ModefiledBy { get; set; }
        public DateTimeOffset ModefiledTime { get; set; }

        public ProductVariantAddedEvent(Guid id, List<ProductVariantEntity> productVariants, Guid modefiledBy)
        {
            Id = id;
            ProductVariants = productVariants ?? throw new ArgumentNullException(nameof(productVariants));
            ModefiledBy = modefiledBy;
            ModefiledTime = DateTimeOffset.UtcNow;
        }
    }
}
