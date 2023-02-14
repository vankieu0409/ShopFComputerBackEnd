using Iot.Core.Extensions;
using MediatR;
using ShopFComputerBackEnd.Order.Domain.Dtos;
using System;

namespace ShopFComputerBackEnd.Order.Infrastructure.Commands.Orders
{
    public class UpdateOrderCommand : IRequest<OrderDto>
    {
        public UpdateOrderCommand(Guid id, string name, bool isEnabled)
        {
            if (id.IsNullOrDefault())
                throw new ArgumentNullException(nameof(id));
            if (name.IsNullOrDefault())
                throw new ArgumentNullException(nameof(name));
            Name = name;
            IsEnabled = isEnabled;
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
    }
}
