using Iot.Core.Shared.IntegrationEvents;
using System;
using System.Collections.Generic;

namespace ShopFComputerBackEnd.Identity.Shared.IntegrationEvents
{
    public class UserUpdateDeviceIntegrationEvent : IntegrationEvent
    {
        public UserUpdateDeviceIntegrationEvent(Guid userId, Guid profileId, string deviceToken)
        {
            UserId = userId;
            ProfileId = profileId;
            DeviceToken = deviceToken;
        }

        public Guid UserId { get; set; }
        public Guid ProfileId { get; set; }
        public string DeviceToken { get; set; }
    }
}
