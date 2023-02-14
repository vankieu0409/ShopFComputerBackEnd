using Iot.Core.Extensions;
using MediatR;
using ShopFComputerBackEnd.Product.Domain.Dtos.Products;
using ShopFComputerBackEnd.Product.Domain.Entities;
using System;
using System.Collections.Generic;

namespace ShopFComputerBackEnd.Product.Infrastructure.Commands.Products
{
    public class CreateProductCommand : IRequest<ProductDto>
    {
        public CreateProductCommand(Guid id, Guid createdBy)
        {
            if (id.IsNullOrDefault())
                throw new ArgumentNullException(nameof(id));
            if (createdBy.IsNullOrDefault())
                throw new ArgumentNullException(nameof(createdBy));
            Id = id;
            IsDeleted = false;
            CreatedBy = createdBy;
            CreatedTime = DateTimeOffset.UtcNow;
            ModefiledBy = CreatedBy;
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Brand { get; set; }
        public bool IsDeleted { get; set; }
        public List<ProductVariantEntity> ProductVariants { get; set; }
        public List<OptionEntity> Options { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public Guid ModefiledBy { get; set; }
    }
}
