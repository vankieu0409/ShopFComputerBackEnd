using Iot.Core.Extensions;
using Iot.Core.Shared.IntegrationEvents;
using ShopFComputerBackEnd.Identity.Domain.shared.ValueObjectShared;
using System;

namespace ShopFComputerBackEnd.Identity.Shared.IntegrationEvents
{
    public class UserSignInIntegrationEvent : IntegrationEvent
    {
        public UserSignInIntegrationEvent(Guid id, string phoneNumber, string accessToken, 
                                          string refreshToken, DateTimeOffset accessTokenExpireTime, 
                                          DateTimeOffset refreshTokenExpireTime , DeviceValueObjectShared device)
        {
            Id = id;
            PhoneNumber = phoneNumber;
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            AccessTokenExpireTime = accessTokenExpireTime;
            RefreshTokenExpireTime = refreshTokenExpireTime;
            if(device.IsNullOrDefault())
                device = new DeviceValueObjectShared();
            Device = device;
        }

        public Guid Id { get; set; }
        public string PhoneNumber { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTimeOffset AccessTokenExpireTime { get; set; }
        public DateTimeOffset RefreshTokenExpireTime { get; set; }
        public DeviceValueObjectShared Device { get; set; }
    }
}
