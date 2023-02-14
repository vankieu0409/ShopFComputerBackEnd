using AutoMapper;
using ShopFComputerBackEnd.Order.Data.Repositories.Interfaces;

using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Bson.Serialization.Serializers;
using ShopFComputerBackEnd.Order.Domain.Events.Orders;
using ShopFComputerBackEnd.Order.Domain.ReadModels;

namespace ShopFComputerBackEnd.Order.Infrastructure.Handler.Events;

public class OrderEventHandler : INotificationHandler<OrderInitializedEvent>
{
    private readonly IMapper _mapper;
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderDetailRepository _orderDetailRepository;
    private readonly ILogger<OrderEventHandler> _logger;

    public OrderEventHandler(IMapper mapper, IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository, ILogger<OrderEventHandler> logger)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _orderDetailRepository = orderDetailRepository ?? throw new ArgumentNullException(nameof(orderDetailRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(OrderInitializedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"Event - {notification.Id} - Start Handle {nameof(notification)}");

        var entity = _mapper.Map<OrderReadModel>(notification);
        _logger.LogDebug($"Event - {notification.Id} - Handle {nameof(notification)} \n Payload: \n {JsonSerializer.Serialize(entity)}");

        await _orderRepository.AddAsync(entity);
        await _orderRepository.SaveChangesAsync();
        _logger.LogDebug($"Event - {notification.Id} - End Handle {nameof(entity)}");
    }
}
