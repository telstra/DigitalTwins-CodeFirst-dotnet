using Telstra.Twins.Attributes;
using Telstra.Twins.Semantics;

namespace FactoryExample.Devices
{
    [DigitalTwin(Version = 1, DisplayName = "Factory Production Step: Grinding/Crushing - Interface Model",
        ExtendsModelId = "dtmi:factoryexample:devices:productionstep;1")]
    public class ProductionStepGrinding : ProductionStep
    {
        [TwinProperty(SemanticType = SemanticType.Temperature, Unit = TemperatureUnit.DegreeCelsius, Writable = true)]
        public double? ChassisTemperature { get; set; }

        [TwinProperty(SemanticType = SemanticType.Force, Unit = ForceUnit.Newton)]
        public double? Force { get; set; }

        [TwinProperty(SemanticType = SemanticType.TimeSpan, Unit = TimeUnit.Minute)]
        public int? GrindingTime { get; set; }

        [TwinProperty(SemanticType = SemanticType.Power, Unit = PowerUnit.Kilowatt)]
        public double? PowerUsage { get; set; }

        [TwinProperty(SemanticType = SemanticType.Frequency, Unit = FrequencyUnit.Hertz)]
        public double? Vibration { get; set; }
    }
}
