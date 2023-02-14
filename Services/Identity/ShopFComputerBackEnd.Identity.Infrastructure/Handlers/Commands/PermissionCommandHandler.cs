using AutoMapper;
using ShopFComputerBackEnd.Identity.Data.Repositories.Interfaces;
using ShopFComputerBackEnd.Identity.Domain.Dtos;
using ShopFComputerBackEnd.Identity.Domain.ReadModels;
using ShopFComputerBackEnd.Identity.Infrastructure.Commands.Permissions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Handlers.Commands
{
    class PermissionCommandHandler : IRequestHandler<AssignPermissionCommand, PermissionDto>,
                                     IRequestHandler<UnassignPermissionCommand, PermissionDto>,
                                     IRequestHandler<AssignPermissionCollectionCommand, ICollection<PermissionDto>>
    {
        private readonly IPermissionRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<PermissionCommandHandler> _logger;
        public PermissionCommandHandler(IMapper mapper, IPermissionRepository repository, ILogger<PermissionCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<PermissionDto> Handle(AssignPermissionCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Command - {request.FunctionId} - Start Handle {nameof(request)}");

            var permission = _mapper.Map<PermissionReadModel>(request);

            _logger.LogDebug($"Command - {request.FunctionId} - Handle {nameof(request)} \n Payload: \n {JsonSerializer.Serialize(permission)}");

            await _repository.AddAsync(permission);
            await _repository.SaveChangesAsync();
            _logger.LogDebug($"Command - {request.FunctionId} - End Handle {nameof(request)}");

            return _mapper.Map<PermissionDto>(permission);
        }

        public async Task<PermissionDto> Handle(UnassignPermissionCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Command - {request.FunctionId} - Start Handle {nameof(request)}");

            var permission = await _repository.AsQueryable().FirstOrDefaultAsync(p => Guid.Equals(p.FunctionId, request.FunctionId) && Guid.Equals(p.TypeId, request.TypeId));
            _logger.LogDebug($"Command - {request.FunctionId} - Handle {nameof(request)} \n Payload: \n {JsonSerializer.Serialize(permission)}");

            await _repository.RemoveAsync(permission);
            await _repository.SaveChangesAsync();
            _logger.LogDebug($"Command - {request.FunctionId} - End Handle {nameof(request)}");

            return _mapper.Map<PermissionDto>(permission);
        }

        public async Task<ICollection<PermissionDto>> Handle(AssignPermissionCollectionCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Command - Start Handle {nameof(request)}");

            var permissions = _mapper.Map<ICollection<PermissionReadModel>>(request.CommandCollection);

            _logger.LogDebug($"Command - Handle {nameof(request)} \n Payload: \n {JsonSerializer.Serialize(permissions)}");

            await _repository.AddRangeAsync(permissions);
            await _repository.SaveChangesAsync();
            _logger.LogDebug($"Command - End Handle {nameof(request)}");

            return _mapper.Map<ICollection<PermissionDto>>(permissions);
        }
    }
}
