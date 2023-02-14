using Iot.Core.Domain.Events;
using ShopFComputerBackEnd.Product.Domain.Entities;
using System;
using System.Collections.Generic;

namespace ShopFComputerBackEnd.Product.Domain.Events
{
    public class ProductInitializedEvent : EventBase
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Brand { get; set; }
        public bool IsDeleted { get; set; }
        public List<ProductVariantEntity> ProductVariants { get; set; }
        public List<OptionEntity> Options { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public Guid ModefiledBy { get; set; }

        public ProductInitializedEvent(Guid id, string name, string description, string category, string brand, List<ProductVariantEntity> productVariants, List<OptionEntity> options, Guid createdBy)
        {
            Id = id;
            Name = name;
            Description = description;
            Category = category;
            Brand = brand;
            ProductVariants = productVariants;
            Options = options;
            IsDeleted = false;
            CreatedBy = createdBy;
            ModefiledBy = CreatedBy;
            CreatedTime = DateTimeOffset.UtcNow;
        }
    }
}
