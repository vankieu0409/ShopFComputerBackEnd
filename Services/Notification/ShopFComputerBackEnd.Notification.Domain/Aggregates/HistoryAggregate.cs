using Iot.Core.Domain.AggregateRoots;
using Iot.Core.Extensions;
using ShopFComputerBackEnd.Notification.Domain.Enums;
using ShopFComputerBackEnd.Notification.Domain.Events.Histories;
using ShopFComputerBackEnd.Notification.Domain.ValueObjects;
using System;

namespace ShopFComputerBackEnd.Notification.Domain.Aggregates
{
    public class HistoryAggregate : AggregateRoot<Guid>
    {
        public HistoryAggregate(Guid id)
        {
            if (id.IsNullOrDefault())
                throw new ArgumentNullException("NotificationHistory id");
            Id = id;
        }
        public Guid TemplateId { get; set; }
        public string Content { get; set; }
        public string Action { get; set; }
        public string GenealogyName { get; set; }
        public DateTimeOffset ActionTime { get; set; }
        public NotificationType Type { get; set; }
        public string ConfigurationType { get; set; }
        public NotificationStatus Status { get; set; }
        public string Message { get; set; }
        public DateTimeOffset SentTime { get; set; }
        public string Destination { get; set; }
        public NotificationBuiltValueObject RawData { get; set; }

        public string StreamName => $"History-{Id}";

        public HistoryAggregate Initialize(Guid templateId, string content ,NotificationType type, string configurationType, NotificationStatus status,
            string message, string destination, NotificationBuiltValueObject rawData)
        {
            var @event = new HistoryInitializedEvent(Id, templateId, content, type, configurationType, status, message, destination, rawData);
            AddDomainEvent(@event);
            Apply(@event);
            return this;
        }
        private void Apply(HistoryInitializedEvent @event)
        {
            Id = @event.Id;
            TemplateId = @event.TemplateId;
            Content = @event.Content;
            Type = @event.Type;
            ConfigurationType = @event.ConfigurationType;
            Status = @event.Status;
            Message = @event.Message;
            SentTime = @event.SentTime;
            Destination = @event.Destination;
            RawData = @event.RawData;
        }
    }
}
