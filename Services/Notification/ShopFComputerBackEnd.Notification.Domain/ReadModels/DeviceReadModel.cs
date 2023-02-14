using Iot.Core.Domain.Entities;
using System;

namespace ShopFComputerBackEnd.Notification.Domain.ReadModels
{
    public class DeviceReadModel : Entity
    {
        public DeviceReadModel(string devicetoken, Guid userId, Guid profileId)
        {
            Devicetoken = devicetoken;
            UserId = userId;
            ProfileId = profileId;
        }
        public string Devicetoken { get; set; }
        public Guid UserId { get; set; }
        public Guid ProfileId { get; set; }
    }
}
