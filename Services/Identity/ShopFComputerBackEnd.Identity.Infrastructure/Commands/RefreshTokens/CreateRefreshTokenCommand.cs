using ShopFComputerBackEnd.Identity.Domain.Dtos;
using ShopFComputerBackEnd.Identity.Domain.ValueObjects;
using MediatR;
using System;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Commands.RefreshTokens
{
    public class CreateRefreshTokenCommand : IRequest<RefreshTokenDto>
    {
        public CreateRefreshTokenCommand(Guid id, Guid userId, 
                                        string deviceId, string refreshToken,
                                        DeviceInfoValueObject deviceInfo)
        {
            Id = id;
            UserId = userId;
            DeviceId = deviceId;
            RefreshToken = refreshToken;
            ExpiredTime = DateTimeOffset.UtcNow.AddDays(7);
            DeviceInfo = deviceInfo;
        }

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string DeviceId { get; set; }
        public string RefreshToken { get; set; }
        public DateTimeOffset ExpiredTime { get; set; }
        public DateTimeOffset RevokedTime { get; set; }
        public DeviceInfoValueObject DeviceInfo { get; set; }
    }
}
