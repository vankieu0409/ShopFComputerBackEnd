using AutoMapper;
using Iot.Core.Extensions;
using Iot.Core.Infrastructure.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ShopFComputerBackEnd.Identity.Domain.Dtos.User;
using ShopFComputerBackEnd.Identity.Api.ViewModels;
using ShopFComputerBackEnd.Identity.Infrastructure.Commands.Users;
using ShopFComputerBackEnd.Identity.Infrastructure.Queries.Users;

namespace ShopFComputerBackEnd.Identity.Api.Controllers
{
    [Route("api/User/[controller]")]
    [ApiController]
    public class MeUserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public MeUserController(IMediator iMediator, IMapper iMapper)
        {
            _mediator = iMediator ?? throw new ArgumentNullException(nameof(iMediator));
            _mapper = iMapper ?? throw new ArgumentNullException(nameof(iMapper));
        }
        [HttpPut]
        [EnableQuery]
        [Authorize]
        public async Task<SingleResult<UserDto>> ChangePasswordAsync([FromBody] SelfChangePasswordViewModel changeViewModel)
        {
            if (string.IsNullOrEmpty(changeViewModel.NewPassword))
                throw new ArgumentNullException("Identity Current Password");
            if (string.IsNullOrEmpty(changeViewModel.CurrentPassword))
                throw new ArgumentNullException("Identity New Password");
            if (string.IsNullOrEmpty(changeViewModel.ConfirmPassword))
                throw new ArgumentNullException("Identity Confirm Password");
            if (!string.Equals(changeViewModel.NewPassword, changeViewModel.ConfirmPassword))
                throw new ArgumentNullException("new Password and confirm password dose not match");

            if (Guid.TryParse(User.Claims.FirstOrDefault(claim => string.Equals(claim.Type, ClaimTypes.NameIdentifier))?.Value, out var userId)) { }

            var query = new GetUserDetailByIdQuery(userId);

            var resultQuery = await _mediator.Send(query);
            if (resultQuery.IsNullOrDefault())
                throw new EntityAlreadyExistsException();

            var command = new ChangePasswordCommand(userId);
            _mapper.Map(changeViewModel, command);

            var result = await _mediator.Send(command);
            return SingleResult.Create(result.ToQueryable());

        }

    }
}
