#nullable enable
using System.Text.Json.Serialization;

namespace Telstra.Twins.Models
{
    public abstract class DtdlType : DtdlBase
    {
        protected DtdlType(string baseType, string? id, string? displayName, string? description, string? comment)
            : base(id, displayName, description, comment)
        {
            BaseType = baseType;
        }

        [JsonIgnore] public string BaseType { get; }

        [JsonPropertyName("@type")] public virtual object Type => BaseType;
    }
}
