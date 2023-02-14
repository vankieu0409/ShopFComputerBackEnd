using Iot.Core.Shared.IntegrationEvents;
using System;
using System.Collections.Generic;

namespace ShopFComputerBackEnd.Notification.Shared.IntegrationEvents
{
    public class NotificationDeathDayIntegrationEvent : IntegrationEvent
    {
        public ICollection<Guid> ProfileIdCollection { get; set; }
    }
}
