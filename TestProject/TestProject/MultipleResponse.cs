using System.Text.Json.Serialization;
using TestProject.Models;

namespace TestProject
{
    abstract class MultipleResponse
    {
        public Info Info { get; set; }
    }

    class MultipleResponseCharacter : MultipleResponse
    {
        [JsonPropertyName("results")]
        public List<Character> Characters { get; set; }
    }

    class MultipleResonseEpisode : MultipleResponse
    {
        [JsonPropertyName("results")]
        public List<Episode> Episodes { get; set; }
    }
}
