using Iot.Core.Extensions;
using MediatR;
using ShopFComputerBackEnd.Order.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ShopFComputerBackEnd.Order.Domain.Entities;

namespace ShopFComputerBackEnd.Order.Infrastructure.Commands.Orders
{
    public class CreateOrderCommand : IRequest<OrderDto>
    {
        public CreateOrderCommand(Guid id)
        {
            if (id.IsNullOrDefault())
                throw new ArgumentNullException(nameof(id));
            Id = id;
            OrderDetailCollection = new Collection<OrderDetailEntity>();
        }
        public Guid Id { get; set; }
        public Guid? ProfileId { get; set; }
        public long AmountPay { get; set; }
        public long PayingCustomer { get; set; }
        public string Description { get; set; }
        public long Payments { get; set; }
        public int StatusOrder { get; set; }
        public bool IsEnabled { get; set; }
        public ICollection<OrderDetailEntity> OrderDetailCollection { get; set; }
        public DateTimeOffset CreatedTime { get; set; }

        public Guid? CreatedBy { get; set; }
    }
}
