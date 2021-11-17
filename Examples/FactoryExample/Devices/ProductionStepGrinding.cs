using Telstra.Twins.Attributes;

namespace FactoryExample.Devices
{
    [DigitalTwin(Version = 1, DisplayName = "Factory Production Step: Grinding/Crushing - Interface Model",
        ExtendsModelId = "dtmi:factoryexample:devices:productionstep;1")]
    public class ProductionStepGrinding : ProductionStep
    {
        //[TwinTelemetry(Unit = "degreeCelsius", SemanticType = "Temperature")]
        [TwinTelemetry]
        public double? ChassisTemperature { get; set; }

        //[TwinTelemetry(Unit = "newton", SemanticType = "Force")]
        [TwinTelemetry]
        public double? Force { get; set; }

        //[TwinTelemetry(Unit = "minute", SemanticType = "TimeSpan")]
        [TwinTelemetry]
        public int? GrindingTime { get; set; }

        //[TwinTelemetry(Unit = "kilowatt", SemanticType = "Power")]
        [TwinTelemetry]
        public double? PowerUsage { get; set; }

        //[TwinTelemetry(Unit = "hertz", SemanticType = "Frequency")]
        [TwinTelemetry]
        public double? Vibration { get; set; }
    }
}
