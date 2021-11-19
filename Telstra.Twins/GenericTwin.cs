using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Telstra.Twins.Attributes;

namespace Telstra.Twins
{
    [DigitalTwin(Version = 1, DisplayName = "Generic Twin for Serialization")]
    public class GenericTwin : TwinBase
    {
        [TwinOnlyProperty]
        [JsonPropertyName("Contents")]
        [System.Text.Json.Serialization.JsonExtensionData]
        public IDictionary<string, object> Contents { get; set; } = new Dictionary<string, object>();
    }
}
