using AutoMapper;
using Iot.Core.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShopFComputerBackEnd.Cart.Data.Repositories.Interfaces;
using ShopFComputerBackEnd.Cart.Domain.Dtos;
using ShopFComputerBackEnd.Cart.Domain.ReadModels;
using ShopFComputerBackEnd.Cart.Infrastructure.Commands;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Iot.Core.EventStore.Abstraction.Interfaces;
using ShopFComputerBackEnd.Cart.Domain.Aggregates;

namespace ShopFComputerBackEnd.Cart.Infrastructure.Handler.Commands
{
    public class CartCommandHandler : IRequestHandler<CreateCartCommand, CartDto>,
                                      IRequestHandler<UpdateCartCommand, CartDto>
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IEventStoreService<CartAggregateRoot> _eventStoreService;
        private readonly ILogger<CartCommandHandler> _logger;

        public CartCommandHandler(IMapper mapper, IMediator mediator, IEventStoreService<CartAggregateRoot> eventStoreService, ILogger<CartCommandHandler> logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper)) ;
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _eventStoreService = eventStoreService ?? throw new ArgumentNullException(nameof(eventStoreService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<CartDto> Handle(CreateCartCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Command - {request.Id} - Start Handle {nameof(request)}");
            var aggregateRoot = new CartAggregateRoot(request.Id);
            _logger.LogDebug($"Command - {request.Id} - Handle {nameof(request)} \n Payload: \n {JsonSerializer.Serialize(aggregateRoot)}");
            aggregateRoot.Initialize(request.ProfileId, request.Items, request.CreatedTime);
            _logger.LogDebug($"Command - {request.Id} - Handle {nameof(request)} \n Payload: \n {JsonSerializer.Serialize(aggregateRoot)}");
            await _eventStoreService.StartStreamAsync(aggregateRoot.StreamName, aggregateRoot);
            _logger.LogDebug($"Command - {request.Id} - End Handle {nameof(request)}");
            return _mapper.Map<CartDto>(aggregateRoot);
        }

        public async Task<CartDto> Handle(UpdateCartCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"ProductCommandHandler - {request.Id} - Start Handle {nameof(request)}");

            var aggregateRoot = new CartAggregateRoot(request.Id);
            var aggregateStream = await _eventStoreService.AggregateStreamAsync(aggregateRoot.StreamName);

            _logger.LogDebug($"ProductCommandHandler - Property : CartItems - {request.Id} - Handle {nameof(request)} \n Payload: \n {JsonSerializer.Serialize(aggregateRoot) }");

            aggregateStream.UpdateCartItems(request.Items,request.ModifiedBy);

            _logger.LogDebug($"ProductCommandHandler - Update To EvenStore - {request.Id} - Handle {nameof(request)} \n Payload: \n {JsonSerializer.Serialize(aggregateRoot)}");

            await _eventStoreService.AppendStreamAsync(aggregateRoot.StreamName, aggregateRoot);

            _logger.LogDebug($"ProductCommandHandler - {request.Id} - Handle End {nameof(request)}");

            return _mapper.Map<CartDto>(aggregateStream);
        }
    }
}
