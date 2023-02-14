using Iot.Core.Domain.Entities;
using ShopFComputerBackEnd.Product.Domain.ReadModels.Products;
using ShopFComputerBackEnd.Product.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace ShopFComputerBackEnd.Product.Domain.ReadModels.Options
{
    public class OptionValueReadModel : Entity<Guid>
    {
        public Guid OptionId { get; set; }
        public Guid ProductVariantId { get; set; }
        public string Name { get; set; } // name of option with id option
        public string Value { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsDeleted { get; set; }
        public virtual OptionReadModel Option { get; set; }
        public virtual ProductVariantReadModel ProductVariant { get; set; }
    }
}
