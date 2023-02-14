using MediatR;

using Microsoft.EntityFrameworkCore.Metadata;
using ShopFComputerBackEnd.Order.Domain.Dtos;

using System.Collections.Generic;

using System;
using ShopFComputerBackEnd.Order.Domain.Entities;

namespace ShopFComputerBackEnd.Order.Infrastructure.Commands.OrderDetails;

public class UpdateOrderDetailsCollectionCommand : IRequest<OrderDto>
{
    public UpdateOrderDetailsCollectionCommand(Guid orderId, ICollection<OrderDetailEntity> orderDetail)
    {
        OrderId = orderId;
        OrderDetail = orderDetail;
    }
    public Guid OrderId { get; set; }
    public ICollection<OrderDetailEntity> OrderDetail { get; set; }
}