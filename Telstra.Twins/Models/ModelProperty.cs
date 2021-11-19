namespace Telstra.Twins.Models
{
    public partial class ModelProperty : Content
    {
        public string SemanticType { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string Id { get; set; }
        public string Comment { get; set; }
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
            bool? writable = null) : this()
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

        public ModelProperty()
        {
            this.Type = "Property";
        }
    }
}