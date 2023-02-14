using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ShopFComputerBackEnd.Notification.Domain.Models
{
    public class DataCalendarModel
    {
        [JsonPropertyName("data")]
        public Data Data { get; set; }
    }
    public class Data
    {
        [JsonPropertyName("profileIds")]
        public ICollection<string> ProfileIdCollection { get; set; }
    }
}
