using ShopFComputerBackEnd.Profile.Domain.Dtos;
using MediatR;

namespace ShopFComputerBackEnd.Profile.Infrastructure.Queries.Profiles
{
    public class GetProfileByDetailPhoneNumberQuery : IRequest<ProfileDto>
    {
        public GetProfileByDetailPhoneNumberQuery(string phoneNumber)
        {
            PhoneNumber = phoneNumber;
        }

        public string PhoneNumber { get; set; }
    }
}
