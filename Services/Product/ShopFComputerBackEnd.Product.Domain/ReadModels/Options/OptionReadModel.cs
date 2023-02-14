using Iot.Core.Domain.Entities;
using ShopFComputerBackEnd.Product.Domain.ReadModels.Products;
using System;
using System.Collections.Generic;

namespace ShopFComputerBackEnd.Product.Domain.ReadModels.Options
{
    public class OptionReadModel : Entity<Guid>
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsDeleted { get; set; }
        public virtual ICollection<OptionValueReadModel> OptionValues { get; set; }
        public virtual ProductReadModel Product { get; set; }
    }
}
