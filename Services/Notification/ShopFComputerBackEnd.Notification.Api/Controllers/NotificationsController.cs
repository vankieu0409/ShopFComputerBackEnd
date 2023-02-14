using AutoMapper;
using Iot.Core.Extensions;
using ShopFComputerBackEnd.Notification.Api.Policies;
using ShopFComputerBackEnd.Notification.Api.ViewModels;
using ShopFComputerBackEnd.Notification.Api.ViewModels.Notifications;
using ShopFComputerBackEnd.Notification.Api.ViewModels.Templates;
using ShopFComputerBackEnd.Notification.Domain.Dtos;
using ShopFComputerBackEnd.Notification.Domain.Entities;
using ShopFComputerBackEnd.Notification.Infrastructure.Commands.Notifications;
using ShopFComputerBackEnd.Notification.Infrastructure.Commands.NotificationTemplates;
using ShopFComputerBackEnd.Notification.Infrastructure.Queries.Device;
using ShopFComputerBackEnd.Notification.Infrastructure.Queries.Notifications;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Notification.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public NotificationsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpPost("GetDeviceTokenCollection")]
        [AllowAnonymous]
        [EnableQuery]
        public async Task<IQueryable<DeviceDto>> GetDeviceTokenCollectionAsync([FromBody] GetDeviceTokenViewModel viewModel)
        {
            if (viewModel.UserIds.IsNullOrDefault())
                throw new ArgumentNullException("User Ids");
            var query = new GetDeviceCollectionByUserIdCollectionQuery(viewModel.UserIds);
            return await _mediator.Send(query);
        }

        [HttpGet]
        [Authorize(Policy = NotificationsPolicies.View)]
        [EnableQuery]
        public async Task<IQueryable<NotificationDto>> GetAsync()
        {
            var query = new GetNotificationCollectionQuery();
            var result = await _mediator.Send(query);
            return result;
        }

        [HttpGet("{key}")]
        [Authorize(Policy = NotificationsPolicies.View)]
        [EnableQuery]
        public async Task<SingleResult<NotificationDto>> GetAsync([FromODataUri] Guid key)
        {
            var query = new GetNotificationDetailByIdQuery(key);
            var result = await _mediator.Send(query);
            return SingleResult.Create(result.ToQueryable());
        }

        [HttpPost]
        //[Authorize(Policy = NotificationsPolicies.Create)]
        [EnableQuery]
        public async Task<SingleResult<NotificationDto>> PostAsync([FromBody] CreateNotificationViewModel viewModel)
        {
            if (string.IsNullOrEmpty(viewModel.Context))
                throw new ArgumentNullException("Context");
            if (string.IsNullOrEmpty(viewModel.Name))
                throw new ArgumentNullException("Name");
            var id = Guid.NewGuid();
            var command = new CreateNotificationCommand(id);
            if (Guid.TryParse(User.Claims.FirstOrDefault(claim => string.Equals(claim.Type, ClaimTypes.NameIdentifier))?.Value, out var userId))
                command.CreatedBy = userId;
            _mapper.Map(viewModel, command);
            var result = await _mediator.Send(command);
            return SingleResult.Create(result.ToQueryable());
        }

        [HttpPut("{key}")]
        [Authorize(Policy = NotificationsPolicies.Update)]
        [EnableQuery]
        public async Task<SingleResult<NotificationDto>> PutAsync([FromODataUri] Guid key, UpdateNotificationViewModel viewModel)
        {
            if (key.IsNullOrDefault())
                throw new ArgumentNullException("NotificationId");
            if (string.IsNullOrEmpty(viewModel.Context))
                throw new ArgumentNullException("Context");
            if (string.IsNullOrEmpty(viewModel.Name))
                throw new ArgumentNullException("Name");
            var command = new UpdateNotificationCommand(key);
            if (Guid.TryParse(User.Claims.FirstOrDefault(claim => string.Equals(claim.Type, ClaimTypes.NameIdentifier))?.Value, out var userId))
                command.ModifiedBy = userId;
            _mapper.Map(viewModel, command);
            var result = await _mediator.Send(command);
            return SingleResult.Create(result.ToQueryable());
        }

        [HttpDelete("{key}")]
        [Authorize(Policy = NotificationsPolicies.Delete)]
        [EnableQuery]
        public async Task<SingleResult<NotificationDto>> DeleteAsync([FromODataUri] Guid key)
        {
            if (key.IsNullOrDefault())
                throw new ArgumentNullException("NotificationId");
            var command = new DeleteNotificationCommand(key);
            if (Guid.TryParse(User.Claims.FirstOrDefault(claim => string.Equals(claim.Type, ClaimTypes.NameIdentifier))?.Value, out var userId))
                command.DeletedBy = userId;
            var result = await _mediator.Send(command);
            return SingleResult.Create(result.ToQueryable());
        }

        [HttpPost("{key}/recover")]
        [Authorize(Policy = NotificationsPolicies.Update)]
        [EnableQuery]
        public async Task<SingleResult<NotificationDto>> RecoverAsync([FromODataUri] Guid key)
        {
            if (key.IsNullOrDefault())
                throw new ArgumentNullException("NotificationId");
            var command = new RecoverNotificationCommand(key);
            if (Guid.TryParse(User.Claims.FirstOrDefault(claim => string.Equals(claim.Type, ClaimTypes.NameIdentifier))?.Value, out var userId))
                command.ModifiedBy = userId;
            var result = await _mediator.Send(command);
            return SingleResult.Create(result.ToQueryable());
        }

        [HttpPost("{key}/UpdateTemplate")]
        //[Authorize(Policy = NotificationsPolicies.Update)]
        [EnableQuery]
        public async Task<SingleResult<NotificationDto>> UpdateTemplateAsync([FromODataUri] Guid key, [FromBody] UpdateTemplateViewModel viewModel)
        {
            if (key.IsNullOrDefault())
                throw new ArgumentNullException("Notification Id");
            var id = Guid.NewGuid();
            var templateDto = _mapper.Map<NotificationTemplateDto>(viewModel);
            templateDto.Id = id;
            var template = _mapper.Map<NotificationTemplateEntity>(templateDto);
            var command = new UpdateNotificationTemplateCommand(key, template);
            var result = await _mediator.Send(command);
            return SingleResult.Create(result.ToQueryable());
        }

        [HttpPost("{key}/RemoveTemplate")]
        [Authorize(Policy = NotificationsPolicies.Delete)]
        [EnableQuery]
        public async Task<SingleResult<NotificationDto>> RemoveTemplateAsync([FromODataUri] Guid key, [FromBody] RemoveTemplateViewModel viewModel)
        {
            if (key.IsNullOrDefault())
                throw new ArgumentNullException("NotificationId");
            if (string.IsNullOrEmpty(viewModel.LanguageCode))
                throw new ArgumentNullException("Language Code");
            var command = new RemoveNotificationTemplateCommand(key, viewModel.LanguageCode);
            var result = await _mediator.Send(command);
            return SingleResult.Create(result.ToQueryable());
        }
    }
}
