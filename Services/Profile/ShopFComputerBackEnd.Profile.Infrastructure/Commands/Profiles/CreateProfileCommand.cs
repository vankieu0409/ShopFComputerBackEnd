using ShopFComputerBackEnd.Profile.Domain.Dtos;
using ShopFComputerBackEnd.Profile.Domain.Enums;
using ShopFComputerBackEnd.Profile.Domain.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;

namespace ShopFComputerBackEnd.Profile.Infrastructure.Commands.Profiles
{
    public class CreateProfileCommand : IRequest<ProfileDto>
    {
        public CreateProfileCommand()
        {

        }
        public CreateProfileCommand(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public GendersType Gender { get; set; }
        public AvatarValueObject Avatar { get; set; }
        public List<AddressValueObject> Address { get; set; }
        public Guid? CreatedBy { get; set; }
    }
}
