using AutoMapper;
using Iot.Core.Extensions;
using ShopFComputerBackEnd.Identity.Api.ViewModels;
using ShopFComputerBackEnd.Identity.Domain.Dtos;
using ShopFComputerBackEnd.Identity.Infrastructure.Commands.Roles;
using ShopFComputerBackEnd.Identity.Api.Policies;
using ShopFComputerBackEnd.Identity.Infrastructure.Queries.Roles;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ShopFComputerBackEnd.Identity.Domain.Dtos.Role;

namespace ShopFComputerBackEnd.Identity.Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public RolesController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        [EnableQuery]
        [Authorize(Policy = RolePolicies.View)]
        public async Task<IEnumerable<RoleDto>> GetAsync()
        {
            var query = new GetRoleCollectionQuery();
            return await _mediator.Send(query);
        }

        [HttpPost]
        [EnableQuery]
        [Authorize(Policy = RolePolicies.Create)]
        public async Task<SingleResult<RoleDto>> PostAsync([FromBody] CreateRoleViewModel viewModel)
        {
            if (string.IsNullOrEmpty(viewModel.Name))
                throw new ArgumentNullException("Role: Name");
            var id = Guid.NewGuid();
            var command = new CreateRoleCommand(id);

            _mapper.Map(viewModel, command);
            if (Guid.TryParse(User.Claims.FirstOrDefault(claim => string.Equals(claim.Type, ClaimTypes.NameIdentifier))?.Value, out var userId))
                command.CreatedBy = userId;
            var result = await _mediator.Send(command);
            return SingleResult.Create(result.ToQueryable());
        }

        [HttpPut("{key}")]
        [EnableQuery]
        [Authorize(Policy = RolePolicies.Update)]
        public async Task<SingleResult> PutAsync([FromODataUri] Guid key, [FromBody] UpdateRoleViewModel viewModel)
        {

            if (string.IsNullOrEmpty(viewModel.Name))
                throw new ArgumentNullException("Name can not be null");

            var command = new UpdateRoleCommand(key);
            _mapper.Map(viewModel, command);

            var result = await _mediator.Send(command);

            return SingleResult.Create(result.ToQueryable());
        }

        [HttpDelete("{key}")]
        [EnableQuery]
        [Authorize(Policy = RolePolicies.Delete)]
        public async Task<SingleResult<RoleDto>> DeleteAsync([FromODataUri] Guid key)
        {
            if (key.IsNullOrDefault())
                throw new ArgumentNullException("Role Id");
            var command = new DeleteRoleCommand(key);

            if (Guid.TryParse(User.Claims.FirstOrDefault(claim => string.Equals(claim.Type, ClaimTypes.NameIdentifier))?.Value, out var userId))
                command.DeletedBy = userId;

            var result = await _mediator.Send(command);

            return SingleResult.Create(result.ToQueryable());
        }

        [HttpPost("{key}/recover")]
        [EnableQuery]
        [Authorize(Policy = RolePolicies.Update)]
        public async Task<SingleResult<RoleDto>> RecoverAsync([FromODataUri] Guid key)
        {
            if (key.IsNullOrDefault())
                throw new ArgumentNullException("Role Id");

            var command = new RecoverRoleCommand(key);

            if (Guid.TryParse(User.Claims.FirstOrDefault(claim => string.Equals(claim.Type, ClaimTypes.NameIdentifier))?.Value, out var userId))
                command.ModifiedBy = userId;

            var result = await _mediator.Send(command);

            return SingleResult.Create(result.ToQueryable());
        }

        [HttpPost("{key}/assign")]
        [EnableQuery]
        [Authorize(Policy = RolePolicies.AssignUsersToRole)]
        public async Task<SingleResult<AssignRoleToUserDto>> AssignAsync([FromODataUri] Guid key, [FromBody] AssignRoleToUsersViewModel viewModel)
        {
            if (key.IsNullOrDefault())
                throw new ArgumentNullException("User Id");
            var command = new AssignRoleToUserCommand(key);
            _mapper.Map(viewModel, command);
            var result = await _mediator.Send(command);
            return SingleResult.Create(result.ToQueryable());

        }
    }
}
