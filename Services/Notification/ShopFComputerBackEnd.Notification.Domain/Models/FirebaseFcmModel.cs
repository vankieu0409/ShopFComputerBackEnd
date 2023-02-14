using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ShopFComputerBackEnd.Notification.Domain.Models
{
    public class FirebaseFcmModel
    {
        [JsonPropertyName("registration_ids")]
        public ICollection<string> RegistrationIds { get; set; }
        [JsonPropertyName("notification")]
        public FcmNotification Notification { get; set; }
        [JsonPropertyName("data")]
        public object Payload { get; set; }
    }
    public class FcmNotification
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("content")]
        public string Content { get; set; }
        [JsonPropertyName("action")]
        public string Action { get; set; }
        [JsonPropertyName("genealogyName")]
        public string GenealogyName { get; set; }
        [JsonPropertyName("actionTime")]
        public string ActionTime { get; set; }
        [JsonPropertyName("body")]
        public string Body { get; set; }
        [JsonPropertyName("data")]
        public object Data { get; set; }
    }
}

