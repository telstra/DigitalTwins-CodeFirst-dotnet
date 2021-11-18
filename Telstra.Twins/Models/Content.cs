using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Telstra.Twins.Models
{
    public class Content
    {
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public object BaseType { get; set; }

        [JsonProperty("name", Order = -2)]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonProperty("schema", Order = -1)]
        [JsonPropertyName("schema")]
        public object Schema { get; set; }

        [JsonProperty("@type", Order = -3)]
        [JsonPropertyName("@type")]
        public virtual object Type { get { return BaseType; } }
    }
}
