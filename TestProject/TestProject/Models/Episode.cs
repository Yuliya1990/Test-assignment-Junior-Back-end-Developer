using System.Text.Json.Serialization;

namespace TestProject.Models
{
    public class Episode
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("air_date")]
        public string Air_date { get; set; }
        [JsonPropertyName("episode")]
        public string EpisodeCode { get; set; }
        [JsonPropertyName("characters")]
        public string[] Characters { get; set; }
        [JsonPropertyName("url")]
        public string Url { get; set; }
        [JsonPropertyName("created")]
        public DateTime Created { get; set; }
    }
}

