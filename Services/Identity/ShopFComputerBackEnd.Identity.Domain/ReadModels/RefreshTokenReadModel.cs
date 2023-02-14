using Iot.Core.Domain.Entities;
using ShopFComputerBackEnd.Identity.Domain.ValueObjects;
using System;

namespace ShopFComputerBackEnd.Identity.Domain.ReadModels
{
    public class RefreshTokenReadModel : Entity<Guid>
    {
        public RefreshTokenReadModel(Guid id, Guid userId, string deviceId, string refreshToken, DeviceInfoValueObject deviceInfo)
        {
            Id = id;
            UserId = userId;
            DeviceId = deviceId;
            RefreshToken = refreshToken;
            DeviceInfo = deviceInfo;
            ExpiredTime = DateTimeOffset.UtcNow.AddDays(7);
            RevokedTime = DateTimeOffset.UtcNow;
        }
        public Guid UserId { get; set; }
        public string DeviceId { get; set; }
        public string RefreshToken { get; set; }
        public DateTimeOffset ExpiredTime { get; set; }
        public DateTimeOffset RevokedTime { get; set; }
        public DeviceInfoValueObject DeviceInfo { get; set; }
    }
}
