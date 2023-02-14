using MediatR;
using ShopFComputerBackEnd.Identity.Domain.Dtos.User;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Queries.Users
{
    public class GetUserDetailQuery : IRequest<UserDto>
    {
        public GetUserDetailQuery(string user)
        {
            User = user;
        }

        public string User { get; set; }
    }
}
