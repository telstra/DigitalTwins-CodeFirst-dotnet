using System.Collections.Generic;
using FactoryExample.Devices;
using Telstra.Twins;
using Telstra.Twins.Attributes;

namespace FactoryExample.Models
{
    [DigitalTwin(Version = 1, DisplayName = "Factory Production Line - Interface Model")]
    public class ProductionLine : TwinBase
    {
        // ContainsEquipment

        [TwinProperty(Writable = true)] public string? CurrentProductId { get; set; }

        [TwinProperty(Writable = true)] public string? LineId { get; set; }

        [TwinProperty(Writable = true)] public string? LineName { get; set; }

        //[TwinProperty] public ProductionLineStatus LineOperationStatus { get; set; }

        [TwinProperty(Writable = true)] public int? ProductBatchNumber { get; set; }

        [TwinRelationship(DisplayName = "Runs Steps")]
        public IList<ProductionStep> RunsSteps { get; } = new List<ProductionStep>();
    }
}
