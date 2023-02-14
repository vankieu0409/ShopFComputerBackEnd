using System;
using AutoMapper;
using MediatR;
using ShopFComputerBackEnd.Order.Data.Repositories.Interfaces;

namespace ShopFComputerBackEnd.Order.Infrastructure.Handler.Queries;

public class OrderQueryHandler
{
    private readonly IMapper _mapper;
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderDetailRepository _orderDetailRepository;

    public OrderQueryHandler(IMapper mapper, IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _orderDetailRepository = orderDetailRepository?? throw new ArgumentNullException(nameof(orderDetailRepository));
    }
}