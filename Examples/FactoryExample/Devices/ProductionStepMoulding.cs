using Telstra.Twins.Attributes;
using Telstra.Twins.Semantics;

namespace FactoryExample.Devices
{
    [DigitalTwin(Version = 1, DisplayName = "Factory Production Step: Moulding - Interface Model",
        ExtendsModelId = "dtmi:factoryexample:devices:productionstep;1")]
    public class ProductionStepMoulding : ProductionStep
    {
        [TwinProperty(SemanticType = SemanticType.Temperature, Unit = TemperatureUnit.DegreeCelsius,
            Writable = true)]
        public double? ChassisTemperature { get; set; }

        [TwinProperty]
        public double? PowerUsage { get; set; }
    }
}
