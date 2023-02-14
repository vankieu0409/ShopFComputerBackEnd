using AutoMapper;
using Iot.Core.AspNetCore.Http;
using Iot.Core.Extensions;
using Iot.Core.Infrastructure.Exceptions;
using ShopFComputerBackEnd.Profile.Api.Policies;
using ShopFComputerBackEnd.Profile.Api.ViewModels;
using ShopFComputerBackEnd.Profile.Domain.Dtos;
using ShopFComputerBackEnd.Profile.Infrastructure.Commands.Profiles;
using ShopFComputerBackEnd.Profile.Infrastructure.Queries.Profiles;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Extensions;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Profile.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfilesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public ProfilesController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpPost]
        [Authorize(Policy = ProfilePolicies.Create)]
        [EnableQuery]
        public async Task<ResultMessageBase> PostAsync([FromBody] CreateProfileViewModel viewModel)
        {
            if (Guid.Equals(viewModel.UserId, null))
                throw new ArgumentNullException("Profile UserId");
            if (string.IsNullOrEmpty(viewModel.DisplayName))
                throw new ArgumentNullException("Profile DisplayName");
            var id = Guid.NewGuid();
            var command = new CreateProfileCommand(id);
            _mapper.Map(viewModel, command);
            if (Guid.TryParse(User.Claims.FirstOrDefault(claim => string.Equals(claim.Type, ClaimTypes.NameIdentifier))?.Value, out var userId))
                command.CreatedBy = userId;

            var result = await _mediator.Send(command);
            return SingleResultMessage.Success(result);
        }

        [HttpPut("{key}")]
        [Authorize(Policy = ProfilePolicies.Update)]
        [EnableQuery]
        public async Task<ResultMessageBase> PutAsync([FromODataUri] Guid key, [FromBody] UpdateProfileViewModel viewModel)
        {

            if (Guid.Equals(viewModel.UserId, null))
                throw new ArgumentNullException("Profile UserId");

            if (key.IsNullOrDefault())
                throw new ArgumentNullException("Profile id");

            if (string.IsNullOrEmpty(viewModel.DisplayName))
                throw new ArgumentNullException("Display Name");

            var command = new UpdateProfileCommand(key);
            _mapper.Map(viewModel, command);
            if (Guid.TryParse(User.Claims.FirstOrDefault(claim => string.Equals(claim.Type, ClaimTypes.NameIdentifier))?.Value, out var userId))
                command.ModifiedBy = userId;
            var result = await _mediator.Send(command);
            return SingleResultMessage.Success(result);
        }

        [HttpDelete("{key}")]
        [Authorize(Policy = ProfilePolicies.Delete)]
        [EnableQuery]
        public async Task<ResultMessageBase> DeleteAsync([FromODataUri] Guid key)
        {
            if (key.IsNullOrDefault())
                throw new ArgumentNullException("Profile id");
            var command = new DeleteProfileCommand(key);
            if (Guid.TryParse(User.Claims.FirstOrDefault(claim => string.Equals(claim.Type, ClaimTypes.NameIdentifier))?.Value, out var userId))
                command.DeletedBy = userId;
            var result = await _mediator.Send(command);
            return SingleResultMessage.Success(result);
        }

        [HttpPost("{key}/Recover")]
        [Authorize(Policy = ProfilePolicies.Update)]
        [EnableQuery]
        public async Task<ResultMessageBase> RecoverAsync([FromODataUri] Guid key)
        {
            if (key.IsNullOrDefault())
                throw new ArgumentNullException("Profile id");

            var command = new RecoverProfileCommand(key);
            if (Guid.TryParse(User.Claims.FirstOrDefault(claim => string.Equals(claim.Type, ClaimTypes.NameIdentifier))?.Value, out var userId))
                command.ModifiedBy = userId;
            var result = await _mediator.Send(command);
            return SingleResultMessage.Success(result);
        }

        [HttpGet("{key}")]
        [Authorize(Policy = ProfilePolicies.View)]
        [EnableQuery]
        public async Task<ResultMessageBase> GetAsync([FromODataUri] Guid key)
        {
            if (key.IsNullOrDefault())
                throw new ArgumentNullException("Profile id");
            var query = new GetProfileDetailByIdQuery(key);
            var result = await _mediator.Send(query);
            if (result.IsNullOrDefault())
                throw new EntityNotFoundException();
            return SingleResultMessage.Success(result);
        }
    }
}
