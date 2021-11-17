using System.Collections.Generic;
using Telstra.Twins.Attributes;

namespace FactoryExample.Models
{
    [DigitalTwin(Version = 1, DisplayName = "Digital Factory - Interface Model")]
    public class Factory
    {
        [TwinProperty] public string? Country { get; set; }

        [TwinProperty] public string? FactoryId { get; set; }

        [TwinProperty] public string? FactoryName { get; set; }

        [TwinRelationship(DisplayName = "Has Floors")]
        public List<FactoryFloor> Floors { get; set; } = new();

        [TwinProperty] public string? ZipCode { get; set; }
    }
}
