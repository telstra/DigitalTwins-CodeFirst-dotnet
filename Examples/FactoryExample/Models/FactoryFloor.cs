using System.Collections.Generic;
using Telstra.Twins.Attributes;

namespace FactoryExample.Models
{
    [DigitalTwin(Version = 1, DisplayName = "Digital Factory - Interface Model")]
    public class FactoryFloor
    {
        [TwinProperty] public double? ComfortIndex { get; set; }

        [TwinProperty] public string? FloorId { get; set; }

        [TwinProperty] public string? FloorName { get; set; }

        [TwinRelationship(DisplayName = "Runs Production Lines")]
        public IList<ProductionLine> RunsLines { get; } = new List<ProductionLine>();

        [TwinTelemetry(Unit = "degreeCelsius", SemanticType = "Temperature")]
        public double? Temperature { get; set; }
    }
}
