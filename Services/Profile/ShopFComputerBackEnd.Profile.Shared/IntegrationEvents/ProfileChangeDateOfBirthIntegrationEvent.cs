using Iot.Core.Shared.IntegrationEvents;
using System;

namespace ShopFComputerBackEnd.Profile.Shared.IntegrationEvents
{
    public class ProfileChangeDateOfBirthIntegrationEvent : IntegrationEvent
    {
        public ProfileChangeDateOfBirthIntegrationEvent()
        {
        }

        public ProfileChangeDateOfBirthIntegrationEvent(Guid profileId, string dateOfBirth)
        {
            ProfileId = profileId;
            DateOfBirth = dateOfBirth;
        }

        public Guid ProfileId { get; set; }
        public string DateOfBirth { get; set; }
    }
}
