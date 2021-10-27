using System;
using System.Text.Json.Serialization;
using Azure.DigitalTwins.Core;

namespace Telstra.Twins.Models
{
    [Serializable]
    public class TwinMetadata : DigitalTwinMetadata
    {
        // used to suppress model from component serialization
        [Newtonsoft.Json.JsonIgnore]
        [JsonIgnore]
        public bool IsComponent { get; set; } = false;
    }
}
