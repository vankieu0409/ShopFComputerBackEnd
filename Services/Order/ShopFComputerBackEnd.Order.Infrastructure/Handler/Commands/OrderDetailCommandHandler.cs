using AutoMapper;
using Iot.Core.EventStore.Abstraction.Interfaces;
using Microsoft.Extensions.Logging;
using ShopFComputerBackEnd.Order.Domain.Aggregates;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Iot.Core.Extensions;
using MediatR;
using ShopFComputerBackEnd.Order.Domain.Dtos;
using ShopFComputerBackEnd.Order.Infrastructure.Commands.OrderDetails;
using Iot.Core.Infrastructure.Exceptions;

namespace ShopFComputerBackEnd.Order.Infrastructure.Handler.Commands;

public class OrderDetailCommandHandler : IRequestHandler<UpdateOrderDetailsCollectionCommand,OrderDto>
{
    private readonly IMapper _mapper;
    private readonly ILogger<OrderDetailCommandHandler> _logger;
    private readonly IEventStoreService<OrderAggregateRoot> _eventStore;

    public OrderDetailCommandHandler(IMapper mapper, ILogger<OrderDetailCommandHandler> logger, IEventStoreService<OrderAggregateRoot> eventStoreService)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _eventStore = eventStoreService ?? throw new ArgumentNullException(nameof(eventStoreService));
    }

    public async Task<OrderDto> Handle(UpdateOrderDetailsCollectionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"Command - {request.OrderId} - Start Handle {nameof(request)}");

        var aggregateRoot = new OrderAggregateRoot(request.OrderId);
        var aggregateStream = await _eventStore.AggregateStreamAsync(aggregateRoot.StreamName);

        _logger.LogDebug($"Command - {request.OrderId} - Handle {nameof(request)} \n Payload: \n {JsonSerializer.Serialize(aggregateRoot)}");

        if (aggregateStream.IsNullOrDefault())
            throw new EntityNotFoundException().WithData($"Order - {request.OrderId}");

        aggregateStream.UpdateOrderDetailsCollection(request.OrderDetail, request.OrderId);
        await _eventStore.AppendStreamAsync(aggregateStream.StreamName, aggregateStream);
        _logger.LogDebug($"Command - {request.OrderId} - End Handle {nameof(request)}");

        return _mapper.Map<OrderDto>(aggregateStream);
    }
}