using Telstra.Twins.Attributes;
using Telstra.Twins.Core;
using Telstra.Twins.Services;
using Xunit;

namespace Telstra.Twins.Test
{
    public class TwinTelemetryAttributeTests
    {
        public TwinTelemetryAttributeTests()
        {
            var modelLibrary = new ModelLibrary();
            Serializer = new DigitalTwinSerializer(modelLibrary);
        }

        private DigitalTwinSerializer Serializer { get; }

        [Fact]
        public void BasicTelemetryShouldSerialiseToModel()
        {
            var expectedModel = @"{
""@id"": ""dtmi:telstra:twins:test:twinwithtelemetry;1"",
""@type"": ""Interface"",
""@context"": ""dtmi:dtdl:context;2"",
""contents"": [
    {  ""@type"": ""Telemetry"", ""name"": ""measurement"", ""schema"": ""integer"" }
]}";

            var model = Serializer.SerializeModel(typeof(TwinWithTelemetry));

            JsonAssert.Equal(expectedModel, model);
        }

        [DigitalTwin]
        private class TwinWithTelemetry : TwinBase
        {
            [TwinTelemetry] public int Measurement { get; set; }
        }
    }
}
