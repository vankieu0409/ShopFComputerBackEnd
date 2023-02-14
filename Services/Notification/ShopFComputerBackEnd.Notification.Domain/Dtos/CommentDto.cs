using ShopFComputerBackEnd.Notification.Domain.ValueObjects;
using System;

namespace ShopFComputerBackEnd.Notification.Domain.Dtos
{
    public class CommentDto
    {
        public Guid TimelineId { get; set; }
        public string Content { get; set; }
        public Guid ProfileId { get; set; }
        public AttachmentValueObject Attachment { get; set; }
        public Guid CommentId { get; set; }
        public Guid Id { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTimeOffset ModifiedTime { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTimeOffset DeletedTime { get; set; }
    }
}
