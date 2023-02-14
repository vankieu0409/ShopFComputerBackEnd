using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Iot.Core.EventStore.Abstraction.Interfaces;
using Iot.Core.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShopFComputerBackEnd.Order.Data.Repositories.Interfaces;
using ShopFComputerBackEnd.Order.Domain.Aggregates;
using ShopFComputerBackEnd.Order.Domain.Dtos;
using ShopFComputerBackEnd.Order.Domain.ReadModels;
using ShopFComputerBackEnd.Order.Infrastructure.Commands;
using ShopFComputerBackEnd.Order.Infrastructure.Commands.Orders;

namespace ShopFComputerBackEnd.Order.Infrastructure.Handler.Commands
{
    public class OrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderDto>,
                                        IRequestHandler<UpdateOrderCommand, OrderDto>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<OrderCommandHandler> _logger;   
        private readonly IEventStoreService<OrderAggregateRoot> _eventStore;

        public OrderCommandHandler(IMapper mapper, ILogger<OrderCommandHandler> logger, IEventStoreService<OrderAggregateRoot> eventStoreService)
        {
            _mapper = mapper ?? throw  new ArgumentNullException(nameof(mapper)) ;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _eventStore = eventStoreService ?? throw new ArgumentNullException(nameof(eventStoreService));
        }

        public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Command - {request.Id} - Start Handle {nameof(request)}");

            var aggregateRoot = new OrderAggregateRoot(request.Id);

            _logger.LogDebug($"Command - {request.Id} - Handle {nameof(request)} \n Payload: \n {JsonSerializer.Serialize(aggregateRoot)}");

            aggregateRoot.Initialize(request.ProfileId, request.AmountPay, request.PayingCustomer, request.Description, request.Payments, request.StatusOrder, request.IsEnabled, request.CreatedBy);
            await _eventStore.StartStreamAsync(aggregateRoot.StreamName, aggregateRoot);
            _logger.LogDebug($"Command - {request.Id} - End Handle {nameof(request)}");

            return _mapper.Map<OrderDto>(aggregateRoot);
        }

        public Task<OrderDto> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

    }
}
