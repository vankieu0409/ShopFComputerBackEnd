using AutoMapper;
using ShopFComputerBackEnd.Identity.Data.Repositories.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Handlers.Events
{
    public class DeviceEventHandler /*: INotificationHandler<UpdateDeviceEvent>*/
    {
        //private readonly IDeviceRepository _deviceRepository;
        //private readonly IUserRepository _userRepository;
        //private readonly IMapper _mapper;
        //private readonly ILogger<DeviceEventHandler> _logger;

        //public DeviceEventHandler(IDeviceRepository deviceRepository, IUserRepository userRepository, IMapper mapper, ILogger<DeviceEventHandler> logger)
        //{
        //    _deviceRepository = deviceRepository ?? throw new ArgumentNullException(nameof(deviceRepository));
        //    _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        //    _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        //    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        //}

        //public async Task Handle(UpdateDeviceEvent notification, CancellationToken cancellationToken)
        //{
        //    _logger.LogDebug($"Event - {notification.Device.UserId} - Start Handle {nameof(notification)}");

        //    var existDeviceToken = await _deviceRepository.AsQueryable().AnyAsync(device => Guid.Equals(device.UserId, notification.Device.UserId) && string.Equals(device.Devicetoken, notification.Device.DeviceToken));
        //    var device = new DeviceReadModel(notification.Device.DeviceToken, notification.Device.UserId);
        //    _logger.LogDebug($"Event - {notification.Device.UserId} - Handle {nameof(notification)} \n Payload: \n {JsonSerializer.Serialize(device)}");

        //    if (!existDeviceToken)
        //        await _deviceRepository.AddAsync(device);
        //    else
        //        await _deviceRepository.UpdateAsync(device);

        //    await _userRepository.SaveChangesAsync();

        //    _logger.LogDebug($"Event - {notification.Device.UserId} - End Handle {nameof(device)}");
        //}
    }
}
