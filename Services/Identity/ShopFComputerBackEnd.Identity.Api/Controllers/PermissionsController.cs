using AutoMapper;
using Iot.Core.AspNetCore.Exceptions;
using Iot.Core.Extensions;
using Iot.Core.Infrastructure.Exceptions;
using ShopFComputerBackEnd.Identity.Api.ViewModels;
using ShopFComputerBackEnd.Identity.Domain.Dtos;
using ShopFComputerBackEnd.Identity.Infrastructure.Commands.Permissions;
using ShopFComputerBackEnd.Identity.Api.Policies;
using ShopFComputerBackEnd.Identity.Infrastructure.Queries.Permissions;
using ShopFComputerBackEnd.Identity.Infrastructure.Queries.Roles;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopFComputerBackEnd.Identity.Domain.Dtos.Permission;
using ShopFComputerBackEnd.Identity.Infrastructure.Commands.Users;
using System.Collections.ObjectModel;

namespace ShopFComputerBackEnd.Identity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public PermissionsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        [EnableQuery]
        [Authorize(Policy = PermissionPolicies.View)]
        public async Task<SingleResult<PermissionDto>> GetAsync([FromODataUri] Guid typeId, Guid functionId)
        {
            if (typeId.IsNullOrDefault())
                throw new BadRequestException("TypeId can not be null.");
            if (functionId.IsNullOrDefault())
                throw new BadRequestException("functionId can not be null.");
            var query = new GetPermissionByTypeAndFunctionQuery(typeId, functionId);
            var result = await _mediator.Send(query);
            return SingleResult.Create(result.ToQueryable());
        }

        [HttpPost]
        [EnableQuery]
        [Authorize(Policy = PermissionPolicies.Create)]
        public async Task<SingleResult<PermissionDto>> PostAsync([FromBody] CreatePermissionViewModel viewModel)
        {
            if (string.IsNullOrEmpty(viewModel.Type.ToString()))
                throw new ArgumentNullException("Type is null!");
            if (string.IsNullOrEmpty(viewModel.TypeId.ToString()))
                throw new ArgumentNullException("TypeId is null!");
            if (string.IsNullOrEmpty(viewModel.FunctionId.ToString()))
                throw new ArgumentNullException("FunctionId is null!");
            if (string.Equals(viewModel.Type, 2))
            {
                var query = new GetRoleDetailByIdQuery(viewModel.TypeId);
                var role = await _mediator.Send(query);
                if (role.IsNullOrDefault())
                    throw new EntityNotFoundException();
            }
            var permission = await _mediator.Send(new GetPermissionByTypeAndFunctionQuery(viewModel.TypeId, viewModel.FunctionId));
            if (!permission.IsNullOrDefault())
                throw new EntityAlreadyExistsException();
            var command = new AssignPermissionCommand(viewModel.TypeId, viewModel.FunctionId);
            _mapper.Map(viewModel, command);
            var result = await _mediator.Send(command);
            return SingleResult.Create(result.ToQueryable());
        }

        [HttpDelete]
        [EnableQuery]
        [Authorize(Policy = PermissionPolicies.Delete)]
        public async Task<SingleResult<PermissionDto>> DeleteAsync([FromODataUri] Guid typeId, Guid functionId)
        {
            if (typeId.IsNullOrDefault())
                throw new ArgumentNullException("TypeId can not be null.");
            if (functionId.IsNullOrDefault())
                throw new ArgumentNullException("FunctionId can not be null.");
            var command = new UnassignPermissionCommand(typeId, functionId);
            var result = await _mediator.Send(command);
            return SingleResult.Create(result.ToQueryable());
        }

        [HttpPost("Assign")]
        [EnableQuery]
        [Authorize(Policy = PermissionPolicies.AssignUsersToRole)]
        public async Task<IQueryable<RoleDto>> AssignAsync([FromBody] UserRoleViewModel viewModel)
        {
            var userIdGroupCollection = viewModel.UserRoles.GroupBy(ur => ur.UserId);
            var roleIds = viewModel.UserRoles.Select(ur => ur.RoleId).Distinct().ToList();
            var rolesQuery = new GetRoleDetailCollectionByRoleIdCollectionQuery(roleIds);
            var resultQuery = await _mediator.Send(rolesQuery);
            var resultAssignUser = new Collection<RoleDto>();
            foreach (var userIdGroup in userIdGroupCollection)
            {
                var userId = userIdGroup.Key;
                var roleIdCollection = userIdGroup.Select(userRole => userRole.RoleId);
                var roleNames = resultQuery.Where(r => roleIdCollection.Any(ru => Guid.Equals(ru, r))).Select(r=>r.Name).ToList();

                var command = new AddToRoleCommand(userId, roleNames);
                var commandResult = await _mediator.Send(command);
                resultAssignUser.Add(commandResult);
            }
            return resultAssignUser.AsQueryable();
        }
    }
}
