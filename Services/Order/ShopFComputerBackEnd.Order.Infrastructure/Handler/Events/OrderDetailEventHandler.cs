using System;
using System.Linq;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Azure.Core;
using Iot.Core.Infrastructure.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using ShopFComputerBackEnd.Order.Data.Repositories.Interfaces;
using ShopFComputerBackEnd.Order.Domain.Dtos;
using ShopFComputerBackEnd.Order.Domain.Entities;
using ShopFComputerBackEnd.Order.Domain.Events.OrderDetails;

namespace ShopFComputerBackEnd.Order.Infrastructure.Handler.Events;

public class OrderDetailEventHandler: INotificationHandler<OrderDetailsUpdateCollectionEvent>
{
    private readonly IMapper _mapper;
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderDetailRepository _orderDetailRepository;
    private readonly ILogger<OrderDetailEventHandler> _logger;

    public OrderDetailEventHandler(IMapper mapper, IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository, ILogger<OrderDetailEventHandler> logger)
    {
        _mapper = mapper;
        _orderRepository = orderRepository;
        _orderDetailRepository = orderDetailRepository;
        _logger = logger;
    }

    public async Task Handle(OrderDetailsUpdateCollectionEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"Event - {notification.OrderId} - Start Handle {nameof(notification)}");

        var existOrderDetails = _orderDetailRepository.AsQueryable().Where(orderDetail => Guid.Equals(orderDetail.OrderId, notification.OrderId)).ToList();

        _logger.LogDebug($"Event - {notification.OrderId} - Handle {nameof(notification)} \n Payload: \n {JsonSerializer.Serialize(existOrderDetails, new JsonSerializerOptions() { WriteIndented = true, ReferenceHandler = ReferenceHandler.Preserve })}");

        var orderDetailCollectionToAdd = notification.OderDetailsCollection
            .Where(orderDetail => !existOrderDetails.Any(entity => Guid.Equals(entity.ProductVariantId, orderDetail.ProductVariantId) & Guid.Equals(entity.OrderId, orderDetail.OrderId)))
            .Select(orderDetail => new OrderDetailEntity());
        _logger.LogDebug($"Event - {notification.OrderId} - Handle {nameof(notification)} \n Payload: \n {JsonSerializer.Serialize(orderDetailCollectionToAdd)}");

        if (orderDetailCollectionToAdd.Any())
            await _orderDetailRepository.AddRangeAsync(orderDetailCollectionToAdd);
        await _orderDetailRepository.SaveChangesAsync();

        _logger.LogDebug($"Event - {notification.OrderId} - End Handle {nameof(existOrderDetails)}");
    }
}
