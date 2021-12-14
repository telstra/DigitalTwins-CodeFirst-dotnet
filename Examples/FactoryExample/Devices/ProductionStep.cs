using System;
using Telstra.Twins;
using Telstra.Twins.Attributes;

namespace FactoryExample.Devices
{
    [DigitalTwin(Version = 1, DisplayName = "Factory Production Steps - Interface Model")]
    public class ProductionStep : TwinBase
    {
        // ContainsEquipment
        
        [TwinProperty] public bool FinalStep { get; set; }
        
        // HasConnectedDevices

        //[TwinProperty] public ProductionStepStatus OperationStatus { get; set; }

        [TwinProperty] public DateTimeOffset? StartTime { get; set; }

        [TwinProperty] public string? StepId { get; set; }

        [TwinRelationship(DisplayName = "Step Link")]
        public ProductionStep? StepLink { get; set; }

        [TwinProperty] public string? StepName { get; set; }
    }
}
