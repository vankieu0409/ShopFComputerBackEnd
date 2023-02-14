using ShopFComputerBackEnd.Notification.Domain.Dtos;
using ShopFComputerBackEnd.Notification.Domain.Enums;
using ShopFComputerBackEnd.Notification.Domain.ValueObjects;
using MediatR;
using System;

namespace ShopFComputerBackEnd.Notification.Infrastructure.Commands.Histories
{
    public class CreateHistoryCommand : IRequest<HistoryDto>
    {
        public CreateHistoryCommand(Guid id, Guid templateId, string content, NotificationType type, string configurationType, NotificationStatus status, string message, string destination, NotificationBuiltValueObject rawData)
        {
            Id = id;
            TemplateId = templateId;
            Content = content;
            Type = type;
            ConfigurationType = configurationType;
            Status = status;
            Message = message;
            Destination = destination;
            RawData = rawData;
        }

        public Guid Id { get; set; }
        public Guid TemplateId { get; set; }
        public string Content { get; set; }
        public NotificationType Type { get; set; }
        public string ConfigurationType { get; set; }
        public NotificationStatus Status { get; set; }
        public string Message { get; set; }
        public DateTimeOffset SentTime { get; set; }
        public string Destination { get; set; }
        public NotificationBuiltValueObject RawData { get; set; }
    }
}