using Iot.Core.Domain.Entities;
using ShopFComputerBackEnd.Product.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace ShopFComputerBackEnd.Product.Domain.Dtos.Options
{
    public class OptionValueDto
    {
        public Guid Id { get; set; }
        public Guid OptionId { get; set; }
        public Guid ProductVariantId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsDeleted { get; set; }
    }
}
