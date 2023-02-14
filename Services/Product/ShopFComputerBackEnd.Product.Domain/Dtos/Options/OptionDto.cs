using Iot.Core.Domain.Entities;
using System;
using System.Collections.Generic;

namespace ShopFComputerBackEnd.Product.Domain.Dtos.Options
{
    public class OptionDto : Entity<Guid>
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsDeleted { get; set; }
    }
}
