using System.Collections.Generic;
using Telstra.Twins;
using Telstra.Twins.Attributes;

namespace FactoryExample.Models
{
    [DigitalTwin(Version = 1, DisplayName = "Digital Factory - Interface Model")]
    public class FactoryFloor : TwinBase
    {
        [TwinProperty] public double? ComfortIndex { get; set; }

        [TwinProperty] public string? FloorId { get; set; }

        [TwinProperty] public string? FloorName { get; set; }

        //[TwinRelationship(DisplayName = "Runs Production Lines")]
        [TwinRelationship(MaxMultiplicity = 500)]
        public IList<ProductionLine> RunsLines { get; } = new List<ProductionLine>();

        //[TwinTelemetry(Unit = "degreeCelsius", SemanticType = "Temperature")]
        [TwinTelemetry]
        public double? Temperature { get; set; }
    }
}
