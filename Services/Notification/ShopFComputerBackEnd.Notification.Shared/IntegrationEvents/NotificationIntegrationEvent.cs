using Iot.Core.Shared.IntegrationEvents;
using ShopFComputerBackEnd.Notification.Shared.Enums;
using System;
using System.Collections.Generic;

namespace ShopFComputerBackEnd.Notification.Shared.IntegrationEvents
{
    public class NotificationIntegrationEvent : IntegrationEvent
    {
        public List<Guid?> ProfileIds { get; set; }
        public string Context { get; set; }
        public string Action { get; set; }
        public string GenealogyName { get; set; }
        public DateTimeOffset ActionTime { get; set; }
        public string Name { get; set; }
        public NotificationShareType Type { get; set; }
        public Dictionary<string, string> Variables { get; set; }
        public object Payload { get; set; }
        public object Data { get; set; }
    }
}
