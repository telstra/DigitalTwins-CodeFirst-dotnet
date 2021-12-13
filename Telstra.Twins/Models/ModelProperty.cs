#nullable enable

using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Telstra.Twins.Serialization;

namespace Telstra.Twins.Models
{
    public partial class ModelProperty : SemanticContent
    {
        // TODO: Split Property and Telemetry
        
        private const string TypeProperty = "Property";
        private const string TypeTelemetry = "Telemetry";

        private ModelProperty(bool isTelemetry, string name, object schema, string? id, string? displayName,
            string? description, string? comment, string? semanticType, string? unit, bool? writable)
            : base(isTelemetry ? TypeTelemetry : TypeProperty, name, schema, id, displayName, description,
                comment, semanticType, unit)
        {
            Writable = writable;
        }

        [JsonProperty("writable")]
        [JsonPropertyName("writable")]
        public bool? Writable { get; }
    }
}
