using Iot.Core.Extensions;
using MediatR;
using ShopFComputerBackEnd.Cart.Domain.Dtos;
using ShopFComputerBackEnd.Cart.Domain.ReadModels;
using System;
using System.Collections.Generic;

namespace ShopFComputerBackEnd.Cart.Infrastructure.Commands
{
    public class UpdateCartCommand : IRequest<CartDto>
    {
        public UpdateCartCommand(Guid id,Guid modifiedBy)
        {
            if (id.IsNullOrDefault())
                throw new ArgumentNullException(nameof(id));
            ModifiedBy = modifiedBy;
        }
        public Guid Id { get; set; }
        public Guid ModifiedBy { get; set; }
        public ICollection<CartItemValueObject> Items { get; set; }
        public DateTimeOffset ModifiedTime { get; set; }
    }
}
