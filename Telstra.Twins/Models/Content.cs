using System.Text.Json.Serialization;

namespace Telstra.Twins.Models
{
    public partial class Content
    {
        [JsonPropertyName("@type")]
        public string Type { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("schema")]
        public object Schema { get; set; }
    }
}
