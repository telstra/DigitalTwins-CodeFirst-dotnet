using System.Collections.Generic;
using MediatR;
using Telstra.Twins;
using Telstra.Twins.Attributes;
using Telstra.Twins.Semantics;

namespace FactoryExample.Models
{
    [DigitalTwin(Version = 1, DisplayName = "Digital Factory - Interface Model")]
    public class FactoryFloor : TwinBase
    {
        [TwinProperty] public double? ComfortIndex { get; set; }

        [TwinProperty(Writable = true)] public string? FloorId { get; set; }

        // FloorHasRooms
        // FloorHasZones
        
        [TwinProperty(Writable = true)] public string? FloorName { get; set; }

        [TwinRelationship(DisplayName = "Runs Production Lines")]
        public IList<ProductionLine> RunsLines { get; } = new List<ProductionLine>();

        [TwinProperty(SemanticType = SemanticType.Temperature, Unit = TemperatureUnit.DegreeCelsius, Writable = true)]
        public double? Temperature { get; set; }
    }
}
