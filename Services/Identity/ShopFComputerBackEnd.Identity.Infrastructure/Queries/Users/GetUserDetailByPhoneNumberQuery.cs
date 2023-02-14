using MediatR;
using ShopFComputerBackEnd.Identity.Domain.Dtos.User;

namespace ShopFComputerBackEnd.Identity.Infrastructure.Queries.Users
{
    public class GetUserDetailByPhoneNumberQuery : IRequest<UserDto>
    {
        public GetUserDetailByPhoneNumberQuery(string phoneNumber)
        {
            PhoneNumber = phoneNumber;
        }
        public string PhoneNumber { get; set; }
    }
}
