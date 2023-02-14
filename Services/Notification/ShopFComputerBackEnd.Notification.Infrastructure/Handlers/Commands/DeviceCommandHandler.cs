using AutoMapper;
using Iot.Core.EventStore.Abstraction.Interfaces;
using Iot.Core.Extensions;
using ShopFComputerBackEnd.Notification.Data.Repositories.Interfaces;
using ShopFComputerBackEnd.Notification.Domain.Aggregates;
using ShopFComputerBackEnd.Notification.Domain.Dtos;
using ShopFComputerBackEnd.Notification.Domain.ReadModels;
using ShopFComputerBackEnd.Notification.Infrastructure.Commands.Devices;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Notification.Infrastructure.Handlers.Commands
{
    public class DeviceCommandHandler : IRequestHandler<UpdateDeviceCommand, DeviceDto>
    {
        private readonly IMapper _mapper;
        private readonly IEventStoreService<NotificationAggregateRoot> _eventStore;
        private readonly ILogger<DeviceCommandHandler> _logger;
        private readonly IDeviceRepository _deviceRepository;

        public DeviceCommandHandler(IMapper mapper, IEventStoreService<NotificationAggregateRoot> eventStore, ILogger<DeviceCommandHandler> logger, IDeviceRepository deviceRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _deviceRepository = deviceRepository ?? throw new ArgumentNullException(nameof(deviceRepository));
        }
        public async Task<DeviceDto> Handle(UpdateDeviceCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Event - {request.UserId} - Start Command Handle {nameof(request)}");
            var deviceMap = new DeviceDto();
            deviceMap.DeviceToken = request.DeviceToken;
            deviceMap.ProfileId = request.ProfileId;
            deviceMap.UserId = request.UserId;

            var device = _mapper.Map<DeviceReadModel>(deviceMap);

            _logger.LogDebug($"Event - {request.UserId} - Handle {nameof(request)} \n Payload: \n {JsonSerializer.Serialize(request)}");

            var deviceExisted = await _deviceRepository.AsQueryable().FirstOrDefaultAsync(entity => Guid.Equals(entity.UserId, device.UserId) && Guid.Equals(entity.ProfileId, device.ProfileId) && string.Equals(entity.Devicetoken, device.Devicetoken));

            if (deviceExisted.IsNullOrDefault())
                await _deviceRepository.AddAsync(device);
            else
                await _deviceRepository.UpdateAsync(device);

            await _deviceRepository.SaveChangesAsync();
            _logger.LogDebug($"Event - {request.UserId} - End Handle {nameof(device)}");
            return _mapper.Map<DeviceDto>(device);
        }
    }
}
