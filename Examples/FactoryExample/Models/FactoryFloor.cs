using Telstra.Twins.Attributes;

namespace FactoryExample.Models
{
    [DigitalTwin(Version = 1, DisplayName = "Digital Factory - Interface Model")]
    public class FactoryFloor
    {
        [TwinProperty] public string? FloorId { get; set; }

        [TwinProperty] public string? FloorName { get; set; }
    }
}
