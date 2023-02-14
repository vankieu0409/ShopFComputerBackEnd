using Iot.Core.Domain.Entities;
using ShopFComputerBackEnd.Product.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Product.Domain.Entities
{
    public class OptionValueEntity
    {
        public Guid Id { get; set; }
        public Guid OptionId { get; set; }
        public Guid ProductVariantId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public int DisplayOrder { get; set; }
    }
}
