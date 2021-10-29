using System.Text.Json.Serialization;
using Newtonsoft.Json;
namespace Telstra.Twins.Models
{
    public partial class ModelRelationship : Content
    {

        public string Id { get; set; }
        public string Comment { get; set; }
        public string Description { get; set; }
        public string DisplayName { get; set; }

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
            this.Type = "Relationship";
            this.Name = name;
            this.DisplayName = displayName;
            this.Id = id;
            this.Comment = comment;
            this.Description = description;
            this.MaxMultiplicity = maxMultiplicity;
            this.MinMultiplicity = minMultiplicity;
            this.Target = target;
            this.Writable = writable;
        }
    }
}