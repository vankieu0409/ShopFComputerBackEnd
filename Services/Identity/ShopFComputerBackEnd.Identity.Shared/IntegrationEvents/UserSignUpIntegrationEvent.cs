using Iot.Core.Shared.IntegrationEvents;
using System;

namespace ShopFComputerBackEnd.Identity.Shared.IntegrationEvents
{
    public class UserSignUpIntegrationEvent : IntegrationEvent
    {
        public UserSignUpIntegrationEvent(Guid userId, string userName, string email, string phoneNumber, string displayName, Guid? createdBy)
        {
            UserId = userId;
            UserName = userName;
            DisplayName = displayName;
            Email = email;
            PhoneNumber = phoneNumber;
            CreatedBy = createdBy;
            CreatedTime = DateTimeOffset.UtcNow;
        }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public Guid? CreatedBy { get; set; }
    }
}
