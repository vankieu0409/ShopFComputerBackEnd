using AutoMapper;
using Iot.Core.DependencyInjection.Abstraction.Interfaces;
using Iot.Core.EventBus.Base.Abstractions;
using MediatR;
using System;
using System.Threading.Tasks;
using ShopFComputerBackEnd.Identity.Shared.IntegrationEvents;
using ShopFComputerBackEnd.Notification.Data.Repositories.Interfaces;
using ShopFComputerBackEnd.Notification.Infrastructure.Commands.Devices;

namespace ShopFComputerBackEnd.Notification.Infrastructure.Handlers.IntegrationEvents
{
    public class DeviceIntegrationEventHandler : IIntegrationEventHandler<UserUpdateDeviceIntegrationEvent>, ITransientDependency
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IDeviceRepository _repository;

        public DeviceIntegrationEventHandler(IMediator mediator, IMapper mapper, IDeviceRepository repository)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task Handle(UserUpdateDeviceIntegrationEvent @event)
        {
            var command = new UpdateDeviceCommand(@event.DeviceToken, @event.ProfileId, @event.UserId);
            await _mediator.Send(command);
        }
    }
}
