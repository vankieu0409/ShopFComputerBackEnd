using System;

namespace ShopFComputerBackEnd.Identity.Domain.Dtos
{
    public class RefreshTokenDto
    {
        public Guid Id { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTimeOffset RefreshTokenExpireTime { get; set; }
        public DateTimeOffset AccessTokenExpireTime { get; set; }
        public Guid UserId { get; set; }
    }
}
