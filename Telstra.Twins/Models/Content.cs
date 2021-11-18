using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Telstra.Twins.Models
{
    public abstract class Content
    {
        protected Content(string baseType, string name = null, object schema = null)
        {
            BaseType = baseType;
            Name = name;
            Schema = schema;
        }

        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public string BaseType { get; set; }

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
