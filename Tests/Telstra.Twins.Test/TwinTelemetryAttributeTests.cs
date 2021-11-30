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
""@id"": ""dtmi:telstra:twins:test:telemetrytwin;1"",
""@type"": ""Interface"",
""@context"": ""dtmi:dtdl:context;2"",
""contents"": [
    {  ""@type"": ""Telemetry"", ""name"": ""measurement"", ""schema"": ""integer"" }
]}";

            var model = Serializer.SerializeModel(typeof(TelemetryTwin));

            JsonAssert.Equal(expectedModel, model);
        }


        [Fact]
        public void SemanticTelemetryShouldSerialiseToModel()
        {
            var expectedModel = @"{
""@id"": ""dtmi:telstra:twins:test:semantictelemetrytwin;1"",
""@type"": ""Interface"",
""@context"": ""dtmi:dtdl:context;2"",
""contents"": [
    {  
        ""@type"": [""Telemetry"", ""Temperature""], 
        ""name"": ""measurement"", 
        ""schema"": ""integer"", 
        ""unit"": ""degreeCelsius"" 
    }
]}";

            var model = Serializer.SerializeModel(typeof(SemanticTelemetryTwin));

            JsonAssert.Equal(expectedModel, model);
        }

        [DigitalTwin]
        private class SemanticTelemetryTwin : TwinBase
        {
            [TwinTelemetry(SemanticType = "Temperature", Unit = "degreeCelsius")]
            public int Measurement { get; set; }
        }

        [DigitalTwin]
        private class TelemetryTwin : TwinBase
        {
            [TwinTelemetry] public int Measurement { get; set; }
        }
    }
}
