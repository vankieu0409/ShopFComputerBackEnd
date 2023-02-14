using Iot.Core.Domain.Entities;
using ShopFComputerBackEnd.Product.Domain.ReadModels.Options;
using ShopFComputerBackEnd.Product.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace ShopFComputerBackEnd.Product.Domain.ReadModels.Products
{
    public class ProductVariantReadModel : Entity<Guid>
    {
        public Guid ProductId { get; set; }
        public string SkuId { get; set; }
        public long ImportPrice { get; set; }
        public long Price { get; set; }
        public int Quantity { get; set; }
        public bool IsDeleted { get; set; }
        public double Rate { get; set; }
        public double Sale { get; set; }
        public virtual ICollection<ImageValueObject> Images { get; set; }
        public virtual ProductReadModel Product { get; set; }
        public virtual ICollection<OptionValueReadModel> OptionValues { get; set; }
    }
}
