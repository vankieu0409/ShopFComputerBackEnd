namespace ShopFComputerBackEnd.Notification.Domain.Dtos
{
    public class PayloadDto
    {
        public string Type { get; set; }
        public ProfileDto Profile { get; set; }
        public TimelineDto Timeline { get; set; }
        public CommentDto Comment { get; set; }
    }
}
