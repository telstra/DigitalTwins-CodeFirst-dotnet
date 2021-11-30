using System.Text.Json.Serialization;

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
        public string BaseType { get; set; }
      
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("schema")]
        public object Schema { get; set; }

        [JsonPropertyName("@type")]
        public virtual object Type { get { return BaseType; } }
    }
}
