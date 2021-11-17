using System.Collections.Generic;
using FactoryExample.Devices;
using Telstra.Twins.Attributes;

namespace FactoryExample.Models
{
    [DigitalTwin(Version = 1, DisplayName = "Factory Production Line - Interface Model")]
    public class ProductionLine
    {
        [TwinProperty] public string? CurrentProductId { get; set; }

        [TwinProperty] public string? LineId { get; set; }

        [TwinProperty] public string? LineName { get; set; }

        [TwinProperty] public ProductionLineStatus LineOperationStatus { get; set; }

        [TwinProperty] public int? ProductBatchNumber { get; set; }

        [TwinRelationship(DisplayName = "Runs Steps")]
        public IList<ProductionStep> RunsSteps { get; } = new List<ProductionStep>();
    }
}
