using Iot.Core.Domain.Entities;
using ShopFComputerBackEnd.Product.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace ShopFComputerBackEnd.Product.Domain.Entities
{
    public class ProductVariantEntity : Entity<Guid>
    {
        public ProductVariantEntity(Guid id)
        {
            Id = id;
        }
        public Guid ProductId { get; set; }
        public string SkuId { get; set; }
        public decimal ImportPrice { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public double Start { get; set; }
        public double Sale { get; set; }
        public ICollection<ImageValueObject> Images { get; set; }
        public List<OptionValueEntity> OptionValues { get; set; }
    }
}
