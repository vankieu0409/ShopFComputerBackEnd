using MediatR;
using ShopFComputerBackEnd.Identity.Domain.Dtos.User;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Queries.Users
{
    public class GetUserDetailByUserNameQuery : IRequest<UserDto>
    {
        public GetUserDetailByUserNameQuery(string userName)
        {
            UserName = userName;    
        }
        public string UserName { get; private set; }
    }
}
