using AutoMapper;
using AutoMapper.QueryableExtensions;
using ShopFComputerBackEnd.Identity.Data.Repositories.Interfaces;
using ShopFComputerBackEnd.Identity.Domain.Dtos;
using ShopFComputerBackEnd.Identity.Domain.Dtos.Function;
using ShopFComputerBackEnd.Identity.Infrastructure.Queries.Functions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Handlers.Queries.Functions
{
    class FunctionQueryHandler : IRequestHandler<GetFunctionCollectionQuery, IQueryable<FunctionDto>>,
                                 IRequestHandler<GetFunctionDetailsById, FunctionDto>,
                                 IRequestHandler<GetFunctionDetailByServiceAndFuntionQuery, FunctionDto>,
                                 IRequestHandler<CheckFunctionExistedByDictionaryServiceNameAndFunctionNameQuery, IQueryable<CheckFunctionExistedDto>>,
                                 IRequestHandler<GetFunctionCollectionByServiceNameCollectionAndFunctionNameCollectionQuery, FunctionGrpcDto>,
                                 IRequestHandler<GetFunctionCollectionByFunctionIdQuery, IQueryable<FunctionDto>>
    {
        private readonly IFunctionRepository _repository;
        private readonly IMapper _mapper;
        public FunctionQueryHandler(IFunctionRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<FunctionDto> Handle(GetFunctionDetailsById request, CancellationToken cancellationToken)
        {
            var function = await _repository.AsQueryable().AsNoTracking().FirstOrDefaultAsync(f => Guid.Equals(f.Id, request.Id));
            return _mapper.Map<FunctionDto>(function);
        }

        public Task<IQueryable<FunctionDto>> Handle(GetFunctionCollectionQuery request, CancellationToken cancellationToken)
        {
            var result = _repository.AsQueryable().AsNoTracking().ProjectTo<FunctionDto>(_mapper.ConfigurationProvider);
            return Task.FromResult(result);
        }

        public async Task<FunctionDto> Handle(GetFunctionDetailByServiceAndFuntionQuery request, CancellationToken cancellationToken)
        {
            var function = await _repository.AsQueryable().AsNoTracking().FirstOrDefaultAsync(f => string.Equals(f.ServiceName, request.ServiceName) && string.Equals(f.FunctionName, request.FunctionName));
            return _mapper.Map<FunctionDto>(function);
        }

        public async Task<IQueryable<CheckFunctionExistedDto>> Handle(CheckFunctionExistedByDictionaryServiceNameAndFunctionNameQuery request, CancellationToken cancellationToken)
        {
            var getFunctionName = await _repository.AsQueryable().AsNoTracking().Select(entity => entity.FunctionName).ToListAsync();
            var getServiceName = await _repository.AsQueryable().AsNoTracking().Select(entity => entity.ServiceName).ToListAsync();
            var getFunctionNotExisted = request.DictionaryFunction.AsQueryable()
                                               .Where(entity => !entity.Value.Any(entity => getFunctionName.Contains(entity)) && !getServiceName.Contains(entity.Key))
                                               .Select(entity => new CheckFunctionExistedDto()
                                               {
                                                   Key = entity.Key,
                                                   Value = entity.Value
                                               });
            return getFunctionNotExisted;
        }

        public async Task<FunctionGrpcDto> Handle(GetFunctionCollectionByServiceNameCollectionAndFunctionNameCollectionQuery request, CancellationToken cancellationToken)
        {
            var result = new FunctionGrpcDto();
            var functionToUpdate = await _repository.AsQueryable().AsNoTracking().ProjectTo<FunctionDto>(_mapper.ConfigurationProvider)
                                                      .Where(entity => request.ServiceName.Contains(entity.ServiceName) && request.FunctionName.Contains(entity.FunctionName))
                                                      .Select(entity => new FunctionDto()
                                                      {
                                                          Id = entity.Id,
                                                          FunctionName = entity.FunctionName,
                                                          ServiceName = entity.ServiceName
                                                      })
                                                      .ToListAsync();
            result.FunctionToUpdate.AddRange(functionToUpdate);

            var functionToAdd = await _repository.AsQueryable().AsNoTracking().ProjectTo<FunctionDto>(_mapper.ConfigurationProvider)
                                                      .Where(entity => !request.ServiceName.Contains(entity.ServiceName) && !request.FunctionName.Contains(entity.FunctionName))
                                                      .Select(entity => new FunctionDto()
                                                      {
                                                          FunctionName = entity.FunctionName,
                                                          ServiceName = entity.ServiceName
                                                      })
                                                      .ToListAsync();
            result.FunctionToAdd.AddRange(functionToAdd);

            return result;
        }

        public Task<IQueryable<FunctionDto>> Handle(GetFunctionCollectionByFunctionIdQuery request, CancellationToken cancellationToken)
        {
            var function = _repository.AsQueryable().AsNoTracking().ProjectTo<FunctionDto>(_mapper.ConfigurationProvider).Where(entity => request.FunctionIdCollection.Contains(entity.Id));
            return Task.FromResult(function);
        }
    }
}
