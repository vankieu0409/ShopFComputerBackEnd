using Iot.Core.Domain.Entities;
using ShopFComputerBackEnd.Product.Domain.Dtos.Options;
using ShopFComputerBackEnd.Product.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace ShopFComputerBackEnd.Product.Domain.Dtos.Products
{
    public class ProductVariantDto : Entity<Guid>
    {
        public Guid ProductId { get; set; }
        public string SkuId { get; set; }
        public long ImportPrice { get; set; }
        public long Price { get; set; }
        public int Quantity { get; set; }
        public double Rate { get; set; }
        public double Sale { get; set; }
        public ICollection<ImageValueObject> Images { get; set; }
        public virtual ICollection<OptionValueDto> OptionValues { get; set; }
    }
}
