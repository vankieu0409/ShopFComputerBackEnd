using ShopFComputerBackEnd.Profile.Domain.Enums;
using ShopFComputerBackEnd.Profile.Domain.ValueObjects;

namespace ShopFComputerBackEnd.Profile.Api.ViewModels
{
    public class ProfileViewModelBase
    {
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public GendersType Gender { get; set; }
        public AvatarValueObject Avatar { get; set; }
        public AddressValueObject Address { get; set; }
    }
}
