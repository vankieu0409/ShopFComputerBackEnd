using ShopFComputerBackEnd.Identity.Domain.Dtos;
using ShopFComputerBackEnd.Identity.Domain.ReadModels;
using MediatR;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Queries.Users
{
    public class GetTokenByRefreshTokenQuery : IRequest<RefreshTokenReadModel>
    {
        public GetTokenByRefreshTokenQuery(string refreshToken)
        {
            RefreshToken = refreshToken;
        }
        public string RefreshToken { get; set; }
    }
}
