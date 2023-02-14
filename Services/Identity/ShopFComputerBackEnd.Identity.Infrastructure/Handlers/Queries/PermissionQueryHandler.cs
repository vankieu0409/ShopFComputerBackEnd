using AutoMapper;
using AutoMapper.QueryableExtensions;
using ShopFComputerBackEnd.Identity.Data.Repositories.Interfaces;
using ShopFComputerBackEnd.Identity.Domain.Dtos;
using ShopFComputerBackEnd.Identity.Infrastructure.Queries.Permissions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Handlers.Queries
{
    class PermissionQueryHandler : IRequestHandler<GetPermissionByTypeAndFunctionQuery, PermissionDto>,
                                    IRequestHandler<GetPermissionByTypeIdQuery, PermissionDto>,
                                    IRequestHandler<GetPermissionCollectionByTypeIdQuery, IQueryable<PermissionDto>>,
                                    IRequestHandler<GetPermissionCollectionByTypeIdCollectionQuery, IQueryable<PermissionDto>>
    {
        private readonly IPermissionRepository _repository;
        private readonly IMapper _mapper;
        public PermissionQueryHandler(IPermissionRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<PermissionDto> Handle(GetPermissionByTypeAndFunctionQuery request, CancellationToken cancellationToken)
        {
            var permission = await _repository.AsQueryable().AsNoTracking().FirstOrDefaultAsync(p => p.TypeId.Equals(request.TypeId) && p.FunctionId.Equals(request.FunctionId));
            return _mapper.Map<PermissionDto>(permission);
        }
        public async Task<PermissionDto> Handle(GetPermissionByTypeIdQuery request, CancellationToken cancellationToken)
        {
            var permission = await _repository.AsQueryable().AsNoTracking().FirstOrDefaultAsync(p => Guid.Equals(p.TypeId, request.TypeId));
            return _mapper.Map<PermissionDto>(permission);
        }
        public Task<IQueryable<PermissionDto>> Handle(GetPermissionCollectionByTypeIdQuery request, CancellationToken cancellationToken)
        {
            var result = _repository.AsQueryable().AsNoTracking().Where(entity => Guid.Equals(entity.TypeId, request.TypeId)).ProjectTo<PermissionDto>(_mapper.ConfigurationProvider);
            return Task.FromResult(result);
        }

        public Task<IQueryable<PermissionDto>> Handle(GetPermissionCollectionByTypeIdCollectionQuery request, CancellationToken cancellationToken)
        {
            var result = _repository.AsQueryable().AsNoTracking().Where(entity => request.TypeId.Contains(entity.TypeId)).ProjectTo<PermissionDto>(_mapper.ConfigurationProvider);
            return Task.FromResult(result);
        }
    }
}
