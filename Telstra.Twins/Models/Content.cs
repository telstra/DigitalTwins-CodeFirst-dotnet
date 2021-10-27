using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Telstra.Twins.Models
{
    public partial class Content
    {
        [JsonProperty("@type", Order = -3)]
        [JsonPropertyName("@type")]
        public string Type { get; set; }

        [JsonProperty("name", Order = -2)]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonProperty("schema", Order = -1)]
        [JsonPropertyName("schema")]
        public string Schema { get; set; }
    }
}
