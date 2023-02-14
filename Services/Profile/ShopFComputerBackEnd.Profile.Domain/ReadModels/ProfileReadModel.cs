using Iot.Core.Domain.Implements;
using ShopFComputerBackEnd.Profile.Domain.Enums;
using ShopFComputerBackEnd.Profile.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace ShopFComputerBackEnd.Profile.Domain.ReadModels
{
    public class ProfileReadModel : FullEntity<Guid>
    {
        public Guid UserId { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public GendersType Gender { get; set; }
        public AvatarValueObject Avatar { get; set; }
        public List<AddressValueObject> Address { get; set; }

    }
}
