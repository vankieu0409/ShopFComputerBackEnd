using Iot.Core.Domain.Implements;
using ShopFComputerBackEnd.Notification.Domain.ValueObjects;
using System;

namespace ShopFComputerBackEnd.Notification.Domain.Dtos
{
    public class ProfileDto 
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string DisplayName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsPrimary { get; set; }
        public string Description { get; set; }
        public AvatarValueObject Avatar { get; set; }
    }
}
