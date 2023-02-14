using Iot.Core.Extensions;
using MediatR;
using ShopFComputerBackEnd.Order.Domain.Dtos;
using System;

namespace ShopFComputerBackEnd.Order.Infrastructure.Queries
{
    public class GetOrderByIdQuery : IRequest<OrderDto>
    {
        public GetOrderByIdQuery(Guid id)
        {
            if (id.IsNullOrDefault())
                throw new ArgumentNullException(nameof(id));
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
