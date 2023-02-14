using Iot.Core.AspNetCore.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using System;
using System.Linq;
using System.Threading.Tasks;
using Iot.Core.Extensions;
using MediatR;
using AutoMapper;
using Iot.Core.AspNetCore.Http;
using ShopFComputerBackEnd.Identity.Api.ViewModels;
using ShopFComputerBackEnd.Identity.Api.Policies;
using ShopFComputerBackEnd.Identity.Infrastructure.Queries.Functions;
using ShopFComputerBackEnd.Identity.Domain.Dtos;
using Iot.Core.Infrastructure.Exceptions;
using ShopFComputerBackEnd.Identity.Infrastructure.Commands.Functions;
using ShopFComputerBackEnd.Identity.Infrastructure.Commands.Permissions;

namespace ShopFComputerBackEnd.Identity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FunctionsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public FunctionsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        [EnableQuery]
        [Authorize(Policy = FunctionPolices.View)]
        public async Task<IQueryable<FunctionDto>> GetAsync()
        {
            return await _mediator.Send(new GetFunctionCollectionQuery());
        }

        [HttpGet("{key}")]
        [EnableQuery]
        [Authorize(Policy = FunctionPolices.View)]
        public async Task<ResultMessageBase> GetAsync([FromODataUri] Guid key)
        {
            if (key.IsNullOrDefault())
                throw new BadRequestException("Id can not be null.");
            var query = new GetFunctionDetailsById(key);
            var result = await _mediator.Send(query);
            return SingleResultMessage.Success(result);
        }

        [HttpPost]
        [EnableQuery]
        [Authorize(Policy = FunctionPolices.Create)]
        public async Task<SingleResult<FunctionDto>> PostAsync([FromBody] CreateFunctionViewModel viewModel)
        {
            if (string.IsNullOrEmpty(viewModel.ServiceName))
                throw new ArgumentNullException("Funtion: Service Name");
            if (string.IsNullOrEmpty(viewModel.FunctionName))
                throw new ArgumentNullException("Function: Function Name");

            var function = await _mediator.Send(new GetFunctionDetailByServiceAndFuntionQuery(viewModel.ServiceName, viewModel.FunctionName));
            if (!function.IsNullOrDefault() && !function.Id.IsNullOrDefault())
                throw new EntityAlreadyExistsException();
            var id = Guid.NewGuid();
            var command = new CreateFunctionCommand(id);
           
            _mapper.Map(viewModel, command);
            var result = await _mediator.Send(command);
            return SingleResult.Create(result.ToQueryable());
        }

        [HttpPut("{key}")]
        [EnableQuery]
        [Authorize(Policy = FunctionPolices.Update)]
        public async Task<SingleResult<FunctionDto>> PutAsync([FromODataUri] Guid key, [FromBody] UpdateFunctionViewModel viewModel)
        {
            if (key.IsNullOrDefault())
                throw new ArgumentNullException("Function id");
            if (string.IsNullOrEmpty(viewModel.ServiceName))
                throw new ArgumentNullException("Function Service Name");
            if (string.IsNullOrEmpty(viewModel.FunctionName))
                throw new ArgumentNullException("Function Function Name");

            var configuration = await _mediator.Send(new GetFunctionDetailByServiceAndFuntionQuery(viewModel.ServiceName, viewModel.FunctionName));
            viewModel.Id = key;
            var command = new UpdateFunctionCommand(key);
            _mapper.Map(viewModel, command);
            var result = await _mediator.Send(command);
            return SingleResult.Create(result.ToQueryable());
        }

        [HttpDelete]
        [EnableQuery]
        [Authorize(Policy = FunctionPolices.Delete)]
        public async Task<SingleResult<FunctionDto>> DeleteAsync([FromODataUri] Guid key)
        {
            if (key.IsNullOrDefault())
                throw new ArgumentNullException("Function id");

            var command = new DeleteFunctionCommand(key);
            var result = await _mediator.Send(command);
            //PushNotification(result);
            return SingleResult.Create(result.ToQueryable());
        }

        [HttpPost("{key}/RemovePermission")]
        [EnableQuery]
        [Authorize(Policy = FunctionPolices.Delete)]
        public async Task<SingleResult<PermissionDto>> RemovePermissionAsync([FromODataUri] Guid key, [FromBody] RemovePermissionViewModel viewModel)
        {
            if (key.IsNullOrDefault())
                throw new ArgumentNullException("Function Id");
            if (viewModel.TypeId.IsNullOrDefault())
                throw new ArgumentNullException("Type Id");
            var command = new UnassignPermissionCommand(viewModel.TypeId, key);
            var result = await _mediator.Send(command);
            return SingleResult.Create(result.ToQueryable());
        }
    }
}
