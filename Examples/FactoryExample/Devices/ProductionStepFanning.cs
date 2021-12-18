using Telstra.Twins.Attributes;
using Telstra.Twins.Semantics;

namespace FactoryExample.Devices
{
    [DigitalTwin(Version = 1, DisplayName = "Factory Production Step: Fanning/Roasting - Interface Model",
        ExtendsModelId = "dtmi:factoryexample:devices:productionstep;1")]
    public class ProductionStepFanning : ProductionStep
    {
        [TwinProperty(SemanticType = SemanticType.Temperature, Unit = TemperatureUnit.DegreeCelsius,
            Writable = true)]
        public double? ChassisTemperature { get; set; }

        [TwinProperty]
        public double? FanSpeed { get; set; }

        [TwinProperty(SemanticType = SemanticType.TimeSpan, Unit = TimeUnit.Minute)]
        public int? RoastingTime { get; set; }

        [TwinProperty(SemanticType = SemanticType.Power, Unit = PowerUnit.Kilowatt)]
        public double? PowerUsage { get; set; }
    }
}
