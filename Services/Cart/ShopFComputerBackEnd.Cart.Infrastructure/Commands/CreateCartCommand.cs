using Iot.Core.Extensions;
using MediatR;
using ShopFComputerBackEnd.Cart.Domain.Dtos;
using ShopFComputerBackEnd.Cart.Domain.ReadModels;
using System;
using System.Collections.Generic;

namespace ShopFComputerBackEnd.Cart.Infrastructure.Commands
{
    public class CreateCartCommand : IRequest<CartDto>
    {
        public CreateCartCommand(Guid id)
        {
            if (id.IsNullOrDefault())
                throw new ArgumentNullException(nameof(id));
            Id = id;
            CreatedTime = DateTimeOffset.UtcNow;
        }
        public Guid Id { get; set; }
        public Guid ProfileId { get; set; }
        public ICollection<CartItemValueObject> Items { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
    }
}
