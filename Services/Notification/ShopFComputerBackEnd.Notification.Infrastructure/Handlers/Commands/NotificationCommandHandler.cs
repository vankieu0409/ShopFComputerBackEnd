using AutoMapper;
using Iot.Core.EventStore.Abstraction.Interfaces;
using Iot.Core.Extensions;
using Iot.Core.Infrastructure.Exceptions;
using ShopFComputerBackEnd.Notification.Domain.Aggregates;
using ShopFComputerBackEnd.Notification.Domain.Dtos;
using ShopFComputerBackEnd.Notification.Infrastructure.Commands.Notifications;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Notification.Infrastructure.Handlers.Commands
{
    public class NotificationCommandHandler : IRequestHandler<CreateNotificationCommand, NotificationDto>,
                                              IRequestHandler<UpdateNotificationCommand, NotificationDto>,
                                                IRequestHandler<DeleteNotificationCommand, NotificationDto>,
                                                IRequestHandler<RecoverNotificationCommand, NotificationDto>
    {
        private readonly IMapper _mapper;
        private readonly IEventStoreService<NotificationAggregateRoot> _eventStore;
        private readonly ILogger<NotificationCommandHandler> _logger;
        public NotificationCommandHandler(IMapper mapper, IEventStoreService<NotificationAggregateRoot> eventStore, ILogger<NotificationCommandHandler> logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<NotificationDto> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Command - {request.Id} - Start Handle {nameof(request)}");

            var aggregateRoot = new NotificationAggregateRoot(request.Id);
            aggregateRoot.Initialize(request.Context, request.Name, request.Template, request.Variables, request.Type, request.CreatedBy);
            _logger.LogDebug($"Command - {request.Id} - Handle {nameof(request)} \n Payload: \n {JsonSerializer.Serialize(aggregateRoot)}");

            await _eventStore.StartStreamAsync(aggregateRoot.StreamName, aggregateRoot);
            _logger.LogDebug($"Command - {request.Id} - End Handle {nameof(request)}");

            return _mapper.Map<NotificationDto>(aggregateRoot);
        }

        public async Task<NotificationDto> Handle(UpdateNotificationCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Command - {request.Id} - Start Handle {nameof(request)}");

            var aggregateRoot = new NotificationAggregateRoot(request.Id);
            var aggregateStream = await _eventStore.AggregateStreamAsync(aggregateRoot.StreamName);

            if (aggregateStream.IsNullOrDefault() || aggregateStream.IsDeleted)
                throw new EntityNotFoundException();

            _logger.LogDebug($"Command - {request.Id} - Handle {nameof(request)} \n Payload: \n {JsonSerializer.Serialize(aggregateStream)}");
            if (!string.Equals(aggregateStream.Name, request.Name))
                aggregateStream.SetName(request.Name, request.ModifiedBy);
            if (aggregateStream.Type != request.Type)
                aggregateStream.SetType(request.Type, request.ModifiedBy);
            aggregateStream.UpdateVariables(request.Variables, request.ModifiedBy);
            _logger.LogDebug($"Command - {request.Id} - Handle {nameof(request)} \n Payload: \n {JsonSerializer.Serialize(aggregateStream)}");

            await _eventStore.AppendStreamAsync(aggregateStream.StreamName, aggregateStream);
            _logger.LogDebug($"Command - {request.Id} - End Handle {nameof(request)}");

            return _mapper.Map<NotificationDto>(aggregateStream);
        }

        public async Task<NotificationDto> Handle(DeleteNotificationCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Command - {request.Id} - Start Handle {nameof(request)}");

            var aggregateRoot = new NotificationAggregateRoot(request.Id);
            var aggregateStream = await _eventStore.AggregateStreamAsync(aggregateRoot.StreamName);

            if (aggregateStream.IsNullOrDefault() || aggregateStream.IsDeleted)
                throw new EntityNotFoundException();

            _logger.LogDebug($"Command - {request.Id} - Handle {nameof(request)} \n Payload: \n {JsonSerializer.Serialize(aggregateStream)}");

            aggregateStream.Delete(request.DeletedBy);
            await _eventStore.AppendStreamAsync(aggregateStream.StreamName, aggregateStream);
            _logger.LogDebug($"Command - {request.Id} - End Handle {nameof(request)}");

            return _mapper.Map<NotificationDto>(aggregateStream);
        }

        public async Task<NotificationDto> Handle(RecoverNotificationCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Command - {request.Id} - Start Handle {nameof(request)}");

            var aggregateRoot = new NotificationAggregateRoot(request.Id);
            var aggregateStream = await _eventStore.AggregateStreamAsync(aggregateRoot.StreamName);

            if (aggregateStream.IsNullOrDefault())
                throw new EntityNotFoundException();
            if (!aggregateStream.IsDeleted)
                throw new EntityAlreadyExistsException();
            _logger.LogDebug($"Command - {request.Id} - Handle {nameof(request)} \n Payload: \n {JsonSerializer.Serialize(aggregateStream)}");

            aggregateStream.Recover(request.ModifiedBy);
            await _eventStore.AppendStreamAsync(aggregateStream.StreamName, aggregateStream);
            _logger.LogDebug($"Command - {request.Id} - End Handle {nameof(request)}");

            return _mapper.Map<NotificationDto>(aggregateStream);
        }
    }
}
