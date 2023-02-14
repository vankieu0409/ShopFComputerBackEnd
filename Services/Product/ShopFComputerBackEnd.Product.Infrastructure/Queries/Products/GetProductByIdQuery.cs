using Iot.Core.Extensions;
using MediatR;
using ShopFComputerBackEnd.Product.Domain.Dtos.Products;
using System;

namespace ShopFComputerBackEnd.Product.Infrastructure.Queries.Products
{
    public class GetProductByUserIdQuery : IRequest<ProductDto>
    {
        public GetProductByUserIdQuery(Guid id)
        {
            if(id.IsNullOrDefault())
                throw new ArgumentNullException(nameof(id));
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
