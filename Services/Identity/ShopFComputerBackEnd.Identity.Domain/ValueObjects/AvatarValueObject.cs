namespace ShopFComputerBackEnd.Identity.Domain.ValueObjects
{
    public class AvatarValueObject
    {
        public string Url { get; set; }
        public string Thumbnail { get; set; }
        public string OriginalFileName { get; set; }
        public int ContentLength { get; set; }
        public string AbsolutePath { get; set; }
        public string FileName { get; set; }
        public string Mime { get; set; }
    }
}
