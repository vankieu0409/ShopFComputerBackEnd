using Iot.Core.Shared.IntegrationEvents;
using System;

namespace ShopFComputerBackEnd.Profile.Shared.IntegrationEvents
{
    public class ProfileChangeDateOfDeathIntegrationEvent : IntegrationEvent
    {
        public ProfileChangeDateOfDeathIntegrationEvent()
        {
        }
        public ProfileChangeDateOfDeathIntegrationEvent(Guid profileId, string dateOfDeath)
        {
            ProfileId = profileId;
            DateOfDeath = dateOfDeath;
        }

        public Guid ProfileId { get; set; }
        public string DateOfDeath { get; set; }
    }
}
