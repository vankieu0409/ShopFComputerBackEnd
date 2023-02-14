using ShopFComputerBackEnd.Identity.Domain.Dtos;
using ShopFComputerBackEnd.Identity.Domain.ValueObjects;
using MediatR;
using System;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Commands.RefreshTokens
{
    public class RevokeRefreshTokenCommand : IRequest<RefreshTokenDto>
    {
        public RevokeRefreshTokenCommand(Guid id)
        {
            Id = id;
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
