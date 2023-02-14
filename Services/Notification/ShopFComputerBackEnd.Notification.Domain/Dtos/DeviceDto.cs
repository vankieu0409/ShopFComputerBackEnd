using System;

namespace ShopFComputerBackEnd.Notification.Domain.Dtos
{
    public class DeviceDto
    {
        public Guid UserId { get; set; }
        public Guid ProfileId { get; set; }
        public string DeviceToken { get; set; }
    }
}
