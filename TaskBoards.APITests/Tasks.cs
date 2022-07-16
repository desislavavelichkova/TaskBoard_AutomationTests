using System.Text.Json.Serialization;

namespace TaskBoard.APITests
{
    public class Tasks
    {
        [JsonPropertyName("id")]
        public int id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

    }
}