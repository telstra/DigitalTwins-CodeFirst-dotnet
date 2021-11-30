namespace Telstra.Twins.Models
using System.Text.Json.Serialization;
using Telstra.Twins.Serialization;

namespace Telstra.Twins.Models
{
    public partial class ModelProperty : Content
    {
        public string Comment { get; set; }
        public string Description { get; set; }
        public string DisplayName { get; set; }
        public string Id { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public string SemanticType { get; set; }

        [JsonProperty("@type", Order = -3)]
        [JsonPropertyName("@type")]
        public override object Type
        {
            get
            {
                if (string.IsNullOrWhiteSpace(SemanticType))
                    return BaseType;
                return new[] { BaseType, SemanticType };
            }
        }

        [JsonProperty("unit")]
        [JsonPropertyName("unit")]
        public string Unit { get; set; }

        public bool? Writable { get; set; }

        public ModelProperty(string name,
            string schema,
            string semanticType = null,
            string displayName = null,
            string description = null,
            string id = null,
            string comment = null,
            string unit = null,
            bool? writable = null) : base("Property")
        {
            this.SemanticType = semanticType;
            this.DisplayName = displayName;
            this.Description = description;
            this.Id = id;
            this.Comment = comment;
            this.Unit = unit;
            this.Writable = writable;
            this.Name = name;
            this.Schema = schema;
        }

        public ModelProperty(): base("Property")
        {
        }
    }
}
