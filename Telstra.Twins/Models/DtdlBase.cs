#nullable enable
using System.Text.Json.Serialization;

namespace Telstra.Twins.Models
{
    public abstract class DtdlBase
    {
        protected DtdlBase(string? id, string? displayName, string? description, string? comment)
        {
            Id = id;
            DisplayName = displayName;
            Description = description;
            Comment = comment;
        }

        [JsonPropertyName("comment")] public string? Comment { get; }

        [JsonPropertyName("description")] public string? Description { get; }

        [JsonPropertyName("displayName")] public string? DisplayName { get; }

        [JsonPropertyName("@id")] public string? Id { get; }
    }
}
