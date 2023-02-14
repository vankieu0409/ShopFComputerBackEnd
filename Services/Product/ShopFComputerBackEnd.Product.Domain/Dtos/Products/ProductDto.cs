using Iot.Core.Domain.Implements;
using ShopFComputerBackEnd.Product.Domain.Dtos.Options;
using System;
using System.Collections.Generic;

namespace ShopFComputerBackEnd.Product.Domain.Dtos.Products
{
    public class ProductDto : FullEntity<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Brand { get; set; }
        public virtual ICollection<OptionDto> Options { get; set; }
        public virtual ICollection<ProductVariantDto> ProductVariants { get; set; }
    }
}
