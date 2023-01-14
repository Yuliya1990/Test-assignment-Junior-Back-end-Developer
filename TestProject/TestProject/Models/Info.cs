using System.Text.Json.Serialization;

namespace TestProject.Models
{
    public class Info
    {
        [JsonPropertyName("count")]
        public int Count { get; set; }
        [JsonPropertyName("pages")]
        public int Pages { get; set; }
        [JsonPropertyName("next")]
        public string Next { get; set; }
        [JsonPropertyName("prev")]
        public string Previous { get; set; }
    }
}
