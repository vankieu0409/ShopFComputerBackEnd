using Iot.Core.Domain.Events;
using ShopFComputerBackEnd.Profile.Domain.Enums;
using ShopFComputerBackEnd.Profile.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace ShopFComputerBackEnd.Profile.Domain.Events
{
    public class ProfileInitializedEvent : EventBase
    {
        public ProfileInitializedEvent(Guid id, Guid userId, string displayName,string email, string phoneNumber, GendersType gender, AvatarValueObject avatar,List<AddressValueObject> address, Guid? createdBy)
        {
            Id = id;
            UserId = userId;
            DisplayName = displayName;
            Email = email;
            PhoneNumber = phoneNumber;
            Gender = gender;
            Avatar = avatar;
            Address = address;
            CreatedBy = createdBy;
            CreatedTime = DateTimeOffset.UtcNow;
        }

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public GendersType Gender { get; set; }
        public AvatarValueObject Avatar { get; set; }
        public string RestingPlace { get; set; }
        public List<AddressValueObject> Address { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
    }
}
