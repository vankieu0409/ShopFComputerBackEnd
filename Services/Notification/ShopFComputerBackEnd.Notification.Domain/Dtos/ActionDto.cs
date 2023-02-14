using System;

namespace ShopFComputerBackEnd.Notification.Domain.Dtos
{
    public class ActionDto
    {
        public string Type { get; set; }
        public Guid ActionId { get; set; }
    }
}
