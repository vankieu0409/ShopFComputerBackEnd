using AutoMapper;
using ShopFComputerBackEnd.Identity.Data.Repositories.Interfaces;
using ShopFComputerBackEnd.Identity.Domain.Dtos;
using ShopFComputerBackEnd.Identity.Domain.ReadModels;
using ShopFComputerBackEnd.Identity.Infrastructure.Commands.Functions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Handlers.Commands
{
    public class FunctionCommandHandler : IRequestHandler<CreateFunctionCommand, FunctionDto>,
        IRequestHandler<UpdateFunctionCommand, FunctionDto>,
        IRequestHandler<DeleteFunctionCommand, FunctionDto>,
        IRequestHandler<CreateFunctionCollectionCommand, ICollection<FunctionDto>>,
        IRequestHandler<UpdateFunctionCollectionCommand, ICollection<FunctionDto>>
    {
        private readonly IFunctionRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<FunctionCommandHandler> _logger;
        public FunctionCommandHandler(IMapper mapper, IFunctionRepository repository, ILogger<FunctionCommandHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        public async Task<FunctionDto> Handle(CreateFunctionCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Command - {request.Id} - Start Handle {nameof(request)}");

            var function = _mapper.Map<FunctionReadModel>(request);

            _logger.LogDebug($"Command - {request.Id} - Handle {nameof(request)} \n Payload: \n {JsonSerializer.Serialize(function)}");

            await _repository.AddAsync(function);
            await _repository.SaveChangesAsync();
            _logger.LogDebug($"Command - {request.Id} - End Handle {nameof(request)}");

            return _mapper.Map<FunctionDto>(function);
        }

        public async Task<FunctionDto> Handle(UpdateFunctionCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Command - {request.Id} - Start Handle {nameof(request)}");

            var function = await _repository.AsQueryable().FirstOrDefaultAsync(f => Guid.Equals(f.Id, request.Id));
            function.ServiceName = request.ServiceName;
            function.FunctionName = request.FunctionName;
            _logger.LogDebug($"Command - {request.Id} - Handle {nameof(request)} \n Payload: \n {JsonSerializer.Serialize(function)}");

            await _repository.UpdateAsync(function);
            await _repository.SaveChangesAsync();
            _logger.LogDebug($"Command - {request.Id} - End Handle {nameof(request)}");

            return _mapper.Map<FunctionDto>(function);
        }

        public async Task<FunctionDto> Handle(DeleteFunctionCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Command - {request.Id} - Start Handle {nameof(request)}");

            var function = await _repository.AsQueryable().FirstOrDefaultAsync(f => Guid.Equals(f.Id, request.Id));
            _logger.LogDebug($"Command - {request.Id} - Handle {nameof(request)} \n Payload: \n {JsonSerializer.Serialize(function)}");

            await _repository.RemoveAsync(function);
            await _repository.SaveChangesAsync();
            _logger.LogDebug($"Command - {request.Id} - End Handle {nameof(request)}");

            return _mapper.Map<FunctionDto>(function);
        }

        public async Task<ICollection<FunctionDto>> Handle(CreateFunctionCollectionCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Command - Start Handle {nameof(request)}");

            var functions = _mapper.Map<ICollection<FunctionReadModel>>(request.CommandCollection);

            _logger.LogDebug($"Command - Handle {nameof(request)} \n Payload: \n {JsonSerializer.Serialize(functions)}");

            await _repository.AddRangeAsync(functions);
            await _repository.SaveChangesAsync();
            _logger.LogDebug($"Command - End Handle {nameof(request)}");

            return _mapper.Map<ICollection<FunctionDto>>(functions);
        }

        public async Task<ICollection<FunctionDto>> Handle(UpdateFunctionCollectionCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Command - Start Handle {nameof(request)}");

            var functions = new Collection<FunctionReadModel>();
            var functionIdCollection = request.CommandCollection.AsQueryable().Select(entity => entity.Id).ToList();

            var functionCollection = _repository.AsQueryable().AsNoTracking().Where(entity => functionIdCollection.Contains(entity.Id));

            foreach (var command in request.CommandCollection)
            {
                var function = functionCollection.FirstOrDefault(entity => Guid.Equals(entity.Id, command.Id));
                function.ServiceName = command.ServiceName;
                function.FunctionName = command.FunctionName;
                functions.Add(function);
            }
            _logger.LogDebug($"Command  - Handle {nameof(request)} \n Payload: \n {JsonSerializer.Serialize(functions)}");

            await _repository.UpdateRangeAsync(functions);
            await _repository.SaveChangesAsync();
            _logger.LogDebug($"Command  - End Handle {nameof(request)}");

            return _mapper.Map<ICollection<FunctionDto>>(functions);
        }
    }
}
