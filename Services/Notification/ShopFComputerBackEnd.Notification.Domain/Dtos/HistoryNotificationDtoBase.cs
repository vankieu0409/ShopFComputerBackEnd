using System;
using System.Collections.Generic;

namespace ShopFComputerBackEnd.Notification.Domain.Dtos
{
    public class HistoryNotificationDtoBase
    {
        public Guid Id { get; set; }
        public string Titile { get; set; }
        public string Content { get; set; }
        public string Action { get; set; }
        public string GenealogyName { get; set; }
        public DateTimeOffset ActionTime { get; set; }
        public ProfileDto ActionProfile { get ; set; }
        public PayloadDto Payload { get; set; }
    }
}
