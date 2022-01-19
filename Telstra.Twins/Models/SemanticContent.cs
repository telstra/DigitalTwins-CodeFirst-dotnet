#nullable enable

using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Telstra.Twins.Models
{
    public abstract class SemanticContent : Content
    {
        protected SemanticContent(string baseType, string name, object schema, string? id,
            string? displayName, string? description, string? comment, string? semanticType, string? unit)
            : base(baseType, name, id, displayName, description, comment)
        {
            Schema = schema;
            SemanticType = semanticType;
            Unit = unit;
        }

        [JsonProperty("schema", Order = -1)]
        [JsonPropertyName("schema")]
        public object Schema { get; }

        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public string? SemanticType { get; }

        [JsonProperty("@type", Order = -3)]
        [JsonPropertyName("@type")]
        public override object Type
        {
            get
            {
                if (string.IsNullOrWhiteSpace(SemanticType))
                {
                    return BaseType;
                }

                return new[] { BaseType, SemanticType };
            }
        }

        [JsonProperty("unit")]
        [JsonPropertyName("unit")]
        public string? Unit { get; }
    }
}
