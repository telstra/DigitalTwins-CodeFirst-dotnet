#nullable enable

using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Telstra.Twins.Models
{
    public partial class ModelRelationship : Content
    {
        // TODO: Support Properties element
        
        private const string TypeRelationship = "Relationship";

        private ModelRelationship(string name, string? id, string? displayName, string? description,
            string? comment, int? maxMultiplicity, int? minMultiplicity, string? target, bool? writable)
            : base(TypeRelationship, name, id, displayName, description, comment)
        {
            MaxMultiplicity = maxMultiplicity;
            MinMultiplicity = minMultiplicity;
            Target = target;
            Writable = writable;
        }

        [JsonPropertyName("maxMultiplicity")]
        [JsonProperty("maxMultiplicity")]
        public int? MaxMultiplicity { get; }

        [JsonPropertyName("minMultiplicity")]
        [JsonProperty("minMultiplicity")]
        public int? MinMultiplicity { get; }

        [JsonProperty("target")]
        [JsonPropertyName("target")]
        public string? Target { get; }

        [JsonProperty("writable")]
        [JsonPropertyName("writable")]
        public bool? Writable { get; }
    }
}
