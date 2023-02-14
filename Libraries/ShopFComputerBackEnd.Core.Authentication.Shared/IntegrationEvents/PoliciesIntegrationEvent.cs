using Iot.Core.Shared.IntegrationEvents;
using System.Collections.Generic;

namespace ShopFComputerBackEnd.Core.Authentication.Shared.IntegrationEvents
{
    public class PoliciesIntegrationEvent : IntegrationEvent
    {
        public PoliciesIntegrationEvent()
        {
        }

        public PoliciesIntegrationEvent(ICollection<PolicyBase> policiesCollection)
        {
            PoliciesCollection = policiesCollection;
        }

        public ICollection<PolicyBase> PoliciesCollection { get; set; }
    }
}
