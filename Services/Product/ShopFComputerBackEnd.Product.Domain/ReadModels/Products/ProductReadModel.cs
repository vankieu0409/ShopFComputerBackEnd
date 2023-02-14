using Iot.Core.Domain.Implements;
using ShopFComputerBackEnd.Product.Domain.ReadModels.Options;
using System;
using System.Collections.Generic;

namespace ShopFComputerBackEnd.Product.Domain.ReadModels.Products
{
    public class ProductReadModel : FullEntity<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Brand { get; set; }
        public virtual ICollection<OptionReadModel> Options { get; set; }
        public virtual ICollection<ProductVariantReadModel> ProductVariants { get; set; }
    }
}
