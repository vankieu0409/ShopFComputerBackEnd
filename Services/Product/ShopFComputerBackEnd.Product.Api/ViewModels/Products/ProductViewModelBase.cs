using ShopFComputerBackEnd.Product.Domain.Entities;
using System.Collections.Generic;

namespace ShopFComputerBackEnd.Product.Api.ViewModels.Products
{
    public class ProductViewModelBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Brand { get; set; }
        public virtual ICollection<OptionEntity> Options { get; set; }
        public virtual ICollection<ProductVariantEntity> ProductVariants { get; set; }
    }
}
