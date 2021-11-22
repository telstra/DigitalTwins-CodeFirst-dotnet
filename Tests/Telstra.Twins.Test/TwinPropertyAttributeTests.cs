using Telstra.Twins.Attributes;
using Telstra.Twins.Core;
using Telstra.Twins.Services;
using Xunit;

namespace Telstra.Twins.Test
{
    public class TwinPropertyAttributeTests
    {
        public TwinPropertyAttributeTests()
        {
            var modelLibrary = new ModelLibrary();
            Serializer = new DigitalTwinSerializer(modelLibrary);
        }

        private DigitalTwinSerializer Serializer { get; }

        [Fact]
        public void BasicPropertyShouldSerialiseToModel()
        {
            var expectedModel = @"{
""@id"": ""dtmi:telstra:twins:test:propertytwin;1"",
""@type"": ""Interface"",
""@context"": ""dtmi:dtdl:context;2"",
""contents"": [
    {  ""@type"": ""Property"", ""name"": ""measurement"", ""schema"": ""integer"" }
]}";

            var model = Serializer.SerializeModel(typeof(PropertyTwin));

            JsonAssert.Equal(expectedModel, model);
        }

        [Fact]
        public void SemanticPropertyShouldSerialiseToModel()
        {
            var expectedModel = @"{
""@id"": ""dtmi:telstra:twins:test:semanticpropertytwin;1"",
""@type"": ""Interface"",
""@context"": ""dtmi:dtdl:context;2"",
""contents"": [
    {
        ""@type"": [""Property"", ""Temperature""],
        ""name"": ""measurement"",
        ""schema"": ""integer"",
        ""unit"": ""degreeCelsius""
    }
]}";

            var model = Serializer.SerializeModel(typeof(SemanticPropertyTwin));

            JsonAssert.Equal(expectedModel, model);
        }

        [DigitalTwin]
        private class PropertyTwin : TwinBase
        {
            [TwinProperty] public int Measurement { get; set; }
        }

        [DigitalTwin]
        private class SemanticPropertyTwin : TwinBase
        {
            [TwinProperty(SemanticType = "Temperature", Unit = "degreeCelsius")]
            public int Measurement { get; set; }
        }
    }
}
