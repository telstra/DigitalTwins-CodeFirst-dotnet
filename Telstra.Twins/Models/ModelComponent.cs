#nullable enable

using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Telstra.Twins.Models
{
    public partial class ModelComponent : Content
    {
        private const string TypeComponent = "Component";

        private ModelComponent(string name, string schema, string? id, string? displayName,
            string? description, string? comment)
            : base(TypeComponent, name, id, displayName, description, comment)
        {
            Schema = schema;
        }

        [JsonProperty("schema", Order = -1)]
        [JsonPropertyName("schema")]
        public object Schema { get; }
    }
}
