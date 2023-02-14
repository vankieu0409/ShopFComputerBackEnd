using AutoMapper;
using AutoMapper.QueryableExtensions;
using ShopFComputerBackEnd.Identity.Data.Repositories.Interfaces;
using ShopFComputerBackEnd.Identity.Domain.Dtos;
using ShopFComputerBackEnd.Identity.Domain.ReadModels;
using ShopFComputerBackEnd.Identity.Infrastructure.Queries.Roles;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Handlers.Queries
{
    public class RoleQueryHandlers : IRequestHandler<GetRoleCollectionQuery, IQueryable<RoleDto>>,
                                     IRequestHandler<GetRoleDetailByNameQuery, RoleDto>,
                                     IRequestHandler<GetRoleDetailByIdQuery, RoleDto>,
                                     IRequestHandler<CheckRoleExistedQuery, ApplicationRoleReadModel>,
                                     IRequestHandler<GetRoleNameCollectionByIdCollectionQuery, List<string>>,
                                     IRequestHandler<GetRoleDetailCollectionByNameCollectionQuery, IQueryable<RoleDto>>,
                                     IRequestHandler<GetRoleDetailCollectionByRoleIdCollectionQuery , IQueryable<RoleDto>>
    {
        private readonly IMapper _mapper;
        private readonly IRoleRepository _repository;
        private readonly RoleManager<ApplicationRoleReadModel> _roleManager;
        public RoleQueryHandlers(IMapper mapper, IRoleRepository repository , RoleManager<ApplicationRoleReadModel> roleManager)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        }
        public Task<IQueryable<RoleDto>> Handle(GetRoleCollectionQuery request, CancellationToken cancellationToken)
        {
            var result = _repository.AsQueryable().AsNoTracking().ProjectTo<RoleDto>(_mapper.ConfigurationProvider);
            return Task.FromResult(result);
        }

        public async Task<RoleDto> Handle(GetRoleDetailByNameQuery request, CancellationToken cancellationToken)
        {
            return await _repository.AsQueryable().AsNoTracking().ProjectTo<RoleDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(entity => Guid.Equals(entity.Name, request.Name));
        }
        public async Task<RoleDto> Handle(GetRoleDetailByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repository.AsQueryable().AsNoTracking().ProjectTo<RoleDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(entity => Guid.Equals(entity.Id, request.Id));
        }
        public async Task<ApplicationRoleReadModel> Handle(CheckRoleExistedQuery request, CancellationToken cancellationToken)
        {
            return await _roleManager.FindByNameAsync(request.RoleName);
        }
        public Task<List<string>> Handle(GetRoleNameCollectionByIdCollectionQuery request, CancellationToken cancellationToken)
        {
            var roleNameCollection= _repository.AsQueryable().AsNoTracking().Where(role => request.RoleIdCollection.Contains(role.Id)).Select(role=> role.Name).ToList();
            return Task.FromResult(roleNameCollection);
        }

        public Task<IQueryable<RoleDto>> Handle(GetRoleDetailCollectionByNameCollectionQuery request, CancellationToken cancellationToken)
        {
            var roleNameCollection = _repository.AsQueryable().AsNoTracking().ProjectTo<RoleDto>(_mapper.ConfigurationProvider).Where(entity => request.NameCollection.Contains(entity.Name));
            return Task.FromResult(roleNameCollection)
;        }

        public Task<IQueryable<RoleDto>> Handle(GetRoleDetailCollectionByRoleIdCollectionQuery request, CancellationToken cancellationToken)
        {
            var roleCollection = _repository.AsQueryable().AsNoTracking().ProjectTo<RoleDto>(_mapper.ConfigurationProvider).Where(entity => request.RoleId.Contains(entity.Id));
            return Task.FromResult(roleCollection);
        }
    }
}
