using ShopFComputerBackEnd.Notification.Domain.Dtos;
using MediatR;
using System;
using System.Collections.Generic;

namespace ShopFComputerBackEnd.Notification.Infrastructure.Commands.Devices
{
    public class UpdateDeviceCommand : IRequest<DeviceDto>
    {
        public UpdateDeviceCommand(string deviceToken, Guid profileId, Guid userId)
        {
            DeviceToken = deviceToken;
            ProfileId = profileId;
            UserId = userId;
        }

        public string DeviceToken { get; set; }
        public Guid ProfileId { get; set; }
        public Guid UserId { get; set; }
    }
}
