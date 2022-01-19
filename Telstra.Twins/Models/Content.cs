#nullable enable
using System.Text.Json.Serialization;

namespace Telstra.Twins.Models
{
    public abstract class Content : DtdlType
    {
        protected Content(string baseType, string name, string? id, string? displayName, string? description,
            string? comment) : base(baseType, id, displayName, description, comment)
        {
            Name = name;
        }

        [Newtonsoft.Json.JsonProperty("name", Order = -2)]
        [JsonPropertyName("name")]
        public string Name { get; }
    }
}
