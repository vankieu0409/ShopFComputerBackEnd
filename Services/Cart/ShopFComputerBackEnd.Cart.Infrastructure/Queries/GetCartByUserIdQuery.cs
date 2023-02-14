using Iot.Core.Extensions;

using MediatR;

using ShopFComputerBackEnd.Cart.Domain.Dtos;

using System;

namespace ShopFComputerBackEnd.Cart.Infrastructure.Queries
{
    public class GetCartByUserIdQuery : IRequest<CartDto>
    {
        public GetCartByUserIdQuery(Guid id)
        {
            if (id.IsNullOrDefault())
                throw new ArgumentNullException(nameof(id));
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
