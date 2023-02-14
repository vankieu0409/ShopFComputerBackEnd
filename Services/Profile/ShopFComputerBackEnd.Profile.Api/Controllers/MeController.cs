using AutoMapper;
using Iot.Core.AspNetCore.Http;
using Iot.Core.EventBus.Base.Abstractions;
using Iot.Core.Extensions;
using Iot.Core.Infrastructure.Exceptions;
using ShopFComputerBackEnd.Profile.Api.ViewModels;
using ShopFComputerBackEnd.Profile.Domain.Dtos;
using ShopFComputerBackEnd.Profile.Infrastructure.Commands.Profiles;
using ShopFComputerBackEnd.Profile.Infrastructure.Queries.Profiles;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Profile.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IEventBus _eventBus;
        private readonly IConfiguration _configuration;

        public MeController(IMediator mediator, IMapper mapper, IEventBus eventBus, IConfiguration configuration)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        [HttpGet("{key}")]
        [Authorize]
        [EnableQuery]
        public async Task<ResultMessageBase> GetAsync([FromODataUri] Guid key)
        {
            if (!Guid.TryParse(User.Claims.FirstOrDefault(claim => string.Equals(claim.Type, ClaimTypes.NameIdentifier))?.Value, out var userId))
                throw new ArgumentNullException("User id");
            if (key.IsNullOrDefault())
                throw new ArgumentNullException("Profile id");

            var result = new ProfileDto();
            var query = new GetProfileDetailByIdQuery(key);
            result = await _mediator.Send(query);
            if (result.IsNullOrDefault())
                throw new EntityNotFoundException();
            return SingleResultMessage.Success(result);
        }

        [HttpPost]
        [Authorize]
        [EnableQuery]
        public async Task<ResultMessageBase> PostAsync(SelfCreateProfileViewModel viewModel)
        {
            if (string.IsNullOrEmpty(viewModel.DisplayName))
                throw new ArgumentNullException("Profile DisplayName");
            var id = Guid.NewGuid();
            var command = new CreateProfileCommand(id);
            _mapper.Map(viewModel, command);
            if (Guid.TryParse(User.Claims.FirstOrDefault(claim => string.Equals(claim.Type, ClaimTypes.NameIdentifier))?.Value, out var userId))
            {
                command.UserId = userId;
                command.CreatedBy = userId;
            }

            var result = await _mediator.Send(command);
            return SingleResultMessage.Success(result);
        }

        [HttpPut("{key}")]
        [Authorize]
        [EnableQuery]
        public async Task<ResultMessageBase> PutAsync([FromODataUri] Guid key,
            [FromBody] SelfUpdateProfileViewModel viewModel)
        {
            if (key.IsNullOrDefault())
                throw new ArgumentNullException("Profile id");

            var userId = Guid.Empty;
            if (!Guid.TryParse(User.Claims.FirstOrDefault(claim => string.Equals(claim.Type, ClaimTypes.NameIdentifier))?.Value, out userId))
                throw new ArgumentNullException("UserId");

            var profile = await _mediator.Send(new GetProfileDetailByIdQuery(userId));

            if (profile.IsNullOrDefault() || profile.IsDeleted)
                throw new EntityNotFoundException();

            var command = new UpdateProfileCommand(key);
            _mapper.Map(viewModel, command);
            command.UserId = userId;
            command.ModifiedBy = userId;

            var result = await _mediator.Send(command);
            return SingleResultMessage.Success(result);
        }

        [HttpDelete("{key}")]
        [Authorize()]
        [EnableQuery]
        public async Task<ResultMessageBase> DeleteAsync([FromODataUri] Guid key)
        {
            if (key.IsNullOrDefault())
                throw new ArgumentNullException("Profile id");
            var command = new DeleteProfileCommand(key);
            if (Guid.TryParse(User.Claims.FirstOrDefault(claim => string.Equals(claim.Type, ClaimTypes.NameIdentifier))?.Value, out var userId)) command.DeletedBy = userId;

            var query = new GetProfileDetailByIdQuery(key);
            var resutlQuery = await _mediator.Send(query);
            if (!Guid.Equals(resutlQuery.UserId, userId))
                throw new EntityNotFoundException();

            var result = await _mediator.Send(command);
            return SingleResultMessage.Success(result);
        }

        [HttpPost("{key}/Recover")]
        [Authorize()]
        [EnableQuery]
        public async Task<ResultMessageBase> RecoverAsync([FromODataUri] Guid key)
        {
            if (key.IsNullOrDefault())
                throw new ArgumentNullException("Profile id");

            var command = new RecoverProfileCommand(key);
            if (Guid.TryParse(User.Claims.FirstOrDefault(claim => string.Equals(claim.Type, ClaimTypes.NameIdentifier))?.Value, out var userId)) command.ModifiedBy = userId;

            var query = new GetProfileDetailByIdQuery(key);
            var resutlQuery = await _mediator.Send(query);
            if (!Guid.Equals(resutlQuery.UserId, userId))
                throw new EntityNotFoundException();

            var result = await _mediator.Send(command);
            return SingleResultMessage.Success(result);
        }
    }
}

