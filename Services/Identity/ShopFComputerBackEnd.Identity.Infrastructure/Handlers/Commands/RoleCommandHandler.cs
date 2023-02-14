using AutoMapper;
using Iot.Core.EventStore.Abstraction.Interfaces;
using Iot.Core.Extensions;
using Iot.Core.Infrastructure.Exceptions;
using ShopFComputerBackEnd.Identity.Domain.Aggregates;
using ShopFComputerBackEnd.Identity.Domain.Dtos;
using ShopFComputerBackEnd.Identity.Infrastructure.Commands.Roles;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Handlers.Commands
{
    class RoleCommandHandler : IRequestHandler<CreateRoleCommand, RoleDto>,
                                IRequestHandler<UpdateRoleCommand, RoleDto>,
                                IRequestHandler<DeleteRoleCommand, RoleDto>,
                                IRequestHandler<RecoverRoleCommand, RoleDto>
    {
        private readonly IMapper _mapper;
        private readonly IEventStoreService<RoleAggregateRoot> _eventStore;
        private readonly ILogger<RoleCommandHandler> _logger;
        public RoleCommandHandler(IMapper mapper, IEventStoreService<RoleAggregateRoot> eventStore, ILogger<RoleCommandHandler> logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<RoleDto> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Command - {request.Id} - Start Handle {nameof(request)}");

            var aggregateRoot = new RoleAggregateRoot(request.Id);
            aggregateRoot.Initialize(request.Name, request.CreatedBy);
            _logger.LogDebug($"Command - {request.Id} - Handle {nameof(request)} \n Payload: \n {JsonSerializer.Serialize(aggregateRoot)}");

            await _eventStore.StartStreamAsync(aggregateRoot.StreamName, aggregateRoot);
            _logger.LogDebug($"Command - {request.Id} - End Handle {nameof(request)}");

            return _mapper.Map<RoleDto>(aggregateRoot);
        }

        public async Task<RoleDto> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Command - {request.Id} - Start Handle {nameof(request)}");
            var aggregateRoot = new RoleAggregateRoot(request.Id);
            var aggregateStream = await _eventStore.AggregateStreamAsync(aggregateRoot.StreamName);
            if (aggregateStream.IsNullOrDefault() || aggregateStream.IsDeleted)
                throw new EntityNotFoundException();
            _logger.LogDebug($"Command - {request.Id} - Handle {nameof(request)} \n Payload: \n {JsonSerializer.Serialize(aggregateRoot)}");
            if (!string.Equals(aggregateStream.Name, request.Name))
                aggregateStream.SetName(request.Name, request.ModifiedBy);
            _logger.LogDebug($"Command - {request.Id} - Handle {nameof(request)} \n Payload: \n {JsonSerializer.Serialize(aggregateRoot)}");

            await _eventStore.AppendStreamAsync(aggregateStream.StreamName, aggregateStream);
            _logger.LogDebug($"Command - {request.Id} - End Handle {nameof(request)}");

            return _mapper.Map<RoleDto>(aggregateStream);
        }

        public async Task<RoleDto> Handle(RecoverRoleCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Command - {request.Id} - Start Handle {nameof(request)}");

            var aggregateRoot = new RoleAggregateRoot(request.Id);
            var aggregateStream = await _eventStore.AggregateStreamAsync(aggregateRoot.StreamName);

            if (aggregateStream.IsNullOrDefault())
                throw new EntityNotFoundException();
            _logger.LogDebug($"Command - {request.Id} - Handle {nameof(request)} \n Payload: \n {JsonSerializer.Serialize(aggregateRoot)}");

            if (aggregateRoot.IsDeleted)
                throw new EntityNotFoundException();
            aggregateStream.Recover(request.ModifiedBy);

            await _eventStore.AppendStreamAsync(aggregateStream.StreamName, aggregateStream);

            _logger.LogDebug($"Command - {request.Id} - End Handle {nameof(request)}");

            return _mapper.Map<RoleDto>(aggregateStream);

        }

        public async Task<RoleDto> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Command - {request.Id} - Start Handle {nameof(request)}");

            var aggregateRoot = new RoleAggregateRoot(request.Id);
            var aggregateStream = await _eventStore.AggregateStreamAsync(aggregateRoot.StreamName);

            if (aggregateStream.IsNullOrDefault() || aggregateStream.IsDeleted)
                throw new EntityNotFoundException();

            _logger.LogDebug($"Command - {request.Id} - Handle {nameof(request)} \n Payload: \n {JsonSerializer.Serialize(aggregateRoot)}");

            aggregateStream.Delete(request.Id);
            await _eventStore.AppendStreamAsync(aggregateStream.StreamName, aggregateStream);
            _logger.LogDebug($"Command - {request.Id} - End Handle {nameof(request)}");
            return _mapper.Map<RoleDto>(aggregateStream);
        }

    }
}
