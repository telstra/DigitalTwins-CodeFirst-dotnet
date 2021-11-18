using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Telstra.Twins.Models
{
    public partial class ModelRelationship : Content
    {
        public ModelRelationship(string name,
            string displayName = null,
            string id = null,
            string comment = null,
            string description = null,
            int? maxMultiplicity = null,
            int? minMultiplicity = null,
            string target = null,
            bool? writable = null)
        {
            BaseType = "Relationship";
            Name = name;
            DisplayName = displayName;
            Id = id;
            Comment = comment;
            Description = description;
            MaxMultiplicity = maxMultiplicity;
            MinMultiplicity = minMultiplicity;
            Target = target;
            Writable = writable;
        }

        public string Comment { get; set; }
        public string Description { get; set; }
        public string DisplayName { get; set; }

        public string Id { get; set; }

        [JsonPropertyName("maxMultiplicity")]
        [JsonProperty("maxMultiplicity")]
        public int? MaxMultiplicity { get; set; }

        [JsonPropertyName("minMultiplicity")]
        [JsonProperty("minMultiplicity")]
        public int? MinMultiplicity { get; set; }

        [JsonProperty("target")]
        [JsonPropertyName("target")]
        public string Target { get; set; }

        public bool? Writable { get; set; }
    }
}
