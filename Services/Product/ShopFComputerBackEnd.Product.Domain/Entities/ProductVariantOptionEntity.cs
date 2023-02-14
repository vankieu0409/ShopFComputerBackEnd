using Iot.Core.Domain.Entities;
using System;
using System.Collections.Generic;

namespace ShopFComputerBackEnd.Product.Domain.Entities
{
    public class ProductVariantOptionEntity : Entity<Guid>
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public List<OptionValueEntity> Values { get; set; }
    }
}
