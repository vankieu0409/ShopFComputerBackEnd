using System;

namespace ShopFComputerBackEnd.Notification.Domain.Dtos
{
    public class TimelineDto
    {
        public Guid Id { get; set; }
        public Guid GroupId { get; set; }
        public string Content { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid ProfileId { get; set; }
    }
}
