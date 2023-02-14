using AutoMapper;
using Iot.Core.EventStore.Abstraction.Interfaces;
using ShopFComputerBackEnd.Notification.Domain.Aggregates;
using ShopFComputerBackEnd.Notification.Domain.Dtos;
using ShopFComputerBackEnd.Notification.Infrastructure.Commands.Histories;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Notification.Infrastructure.Handlers.Commands
{
    public class HistoryCommandHandler : IRequestHandler<CreateHistoryCommand, HistoryDto>
    {
        private readonly IMapper _mapper;
        private readonly IEventStoreService<HistoryAggregate> _eventStore;
        private readonly ILogger<HistoryCommandHandler> _logger;
        public HistoryCommandHandler(IMapper mapper, IEventStoreService<HistoryAggregate> eventStore, ILogger<HistoryCommandHandler> logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<HistoryDto> Handle(CreateHistoryCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Command - {request.Id} - Start Handle {nameof(request)}");

            var aggregateRoot = new HistoryAggregate(request.Id);
            aggregateRoot.Initialize(request.TemplateId, request.Content, request.Type, request.ConfigurationType, request.Status, request.Message, request.Destination, request.RawData);
            _logger.LogDebug($"Command - {request.Id} - Handle {nameof(request)} \n Payload: \n {JsonSerializer.Serialize(aggregateRoot)}");

            await _eventStore.StartStreamAsync(aggregateRoot.StreamName, aggregateRoot);
            _logger.LogDebug($"Command - {request.Id} - End Handle {nameof(request)}");

            return _mapper.Map<HistoryDto>(aggregateRoot);
        }
    }
}
