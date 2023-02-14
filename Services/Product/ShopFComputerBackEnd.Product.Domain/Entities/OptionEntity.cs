using Iot.Core.Domain.Entities;
using System;

namespace ShopFComputerBackEnd.Product.Domain.Entities
{
    public class OptionEntity : Entity<Guid>
    {
        public OptionEntity(Guid id)
        {
            Id = id;
        }
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public int DisplayOrder { get; set; }
    }
}
