using AutoMapper;
using ShopFComputerBackEnd.Identity.Data.Repositories.Interfaces;
using ShopFComputerBackEnd.Identity.Domain.Events.Roles;
using ShopFComputerBackEnd.Identity.Domain.ReadModels;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Handlers.Events
{
    public class RoleEventHandler : INotificationHandler<RoleInitializedEvent>,
                                    INotificationHandler<RoleNameChangedEvent>,
                                    INotificationHandler<RoleDeletedEvent>,
                                    INotificationHandler<RoleRecoveredEvent>
                                
    {
        private readonly IRoleRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<RoleEventHandler> _logger;
        private readonly RoleManager<ApplicationRoleReadModel> _roleManager;
        public RoleEventHandler(IRoleRepository repository, IMapper mapper,
            ILogger<RoleEventHandler> logger, RoleManager<ApplicationRoleReadModel> roleManager)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        }



        public async Task Handle(RoleInitializedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Event - {notification.Id} - Start Handle {nameof(notification)}");

            var role = _mapper.Map<ApplicationRoleReadModel>(notification);
            _logger.LogDebug($"Event - {notification.Id} - Handle {nameof(notification)} \n Payload: \n {JsonSerializer.Serialize(role)}");

            await _roleManager.CreateAsync(role);
            _logger.LogDebug($"Event - {notification.Id} - End Handle {nameof(role)}");
        }

        public async Task Handle(RoleDeletedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Event - {notification.Id} - Start Handle {nameof(notification)}");

            var role = await _repository.AsQueryable().FirstOrDefaultAsync(role => Guid.Equals(role.Id, notification.Id));
            role.DeletedBy = notification.DeletedBy;
            _logger.LogDebug($"Event - {notification.Id} - Handle {nameof(notification)} \n Payload: \n {JsonSerializer.Serialize(role)}");

            await _repository.RemoveAsync(role);
            await _repository.SaveChangesAsync();
            _logger.LogDebug($"Event - {notification.Id} - End Handle {nameof(role)}");
        }

        public async Task Handle(RoleRecoveredEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Event - {notification.Id} - Start Handle {nameof(notification)}");

            var role = await _repository.AsQueryable().FirstOrDefaultAsync(role => Guid.Equals(role.Id, notification.Id));
            role.IsDeleted = false;
            role.ModifiedBy = notification.ModifiedBy;
            _logger.LogDebug($"Event - {notification.Id} - Handle {nameof(notification)} \n Payload: \n {JsonSerializer.Serialize(role)}");

            await _repository.UpdateAsync(role);
            await _repository.SaveChangesAsync();
            _logger.LogDebug($"Event - {notification.Id} - End Handle {nameof(role)}");
        }

        public async Task Handle(RoleNameChangedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Event - {notification.Id} - Start Handle {nameof(notification)}");

            var role = await _repository.AsQueryable().FirstOrDefaultAsync(role => Guid.Equals(role.Id, notification.Id));
            role.Name = notification.Name;
            role.ModifiedBy = notification.ModifiedBy;
            _logger.LogDebug($"Event - {notification.Id} - Handle {nameof(notification)} \n Payload: \n {JsonSerializer.Serialize(role)}");

            await _roleManager.UpdateAsync(role);

            _logger.LogDebug($"Event - {notification.Id} - End Handle {nameof(role)}");
        }
    }
}
