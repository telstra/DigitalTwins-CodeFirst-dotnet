using Telstra.Twins.Attributes;
using Telstra.Twins.Core;
using Telstra.Twins.Semantics;
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
    {  ""@type"": ""Property"", ""name"": ""counter"", ""schema"": ""integer"" }
]}";

            var model = Serializer.SerializeModel(typeof(PropertyTwin));

            JsonAssert.Equal(expectedModel, model);
        }

        [Fact]
        public void PropertySchemaOverrideShouldSerialiseToModel()
        {
            var expectedModel = @"{
  ""@id"": ""dtmi:telstra:twins:test:twinwithschemaoverride;1"",
  ""@type"": ""Interface"",
  ""@context"": ""dtmi:dtdl:context;2"",
  ""contents"": [
    {
      ""@type"": ""Property"",
      ""name"": ""manufacturedDateOnly"",
      ""schema"": ""date""
    }
  ]
}";

            var model = Serializer.SerializeModel(typeof(TwinWithSchemaOverride));

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
        ""name"": ""target"",
        ""schema"": ""integer"",
        ""unit"": ""degreeCelsius""
    }
]}";

            var model = Serializer.SerializeModel(typeof(SemanticPropertyTwin));

            JsonAssert.Equal(expectedModel, model);
        }

        [Fact]
        public void WriteablePropertyShouldSerialiseToModel()
        {
            var expectedModel = @"{
""@id"": ""dtmi:telstra:twins:test:writablepropertytwin;1"",
""@type"": ""Interface"",
""@context"": ""dtmi:dtdl:context;2"",
""contents"": [
    {  ""@type"": ""Property"", ""name"": ""measurement"", ""schema"": ""integer"", ""writable"": true }
]}";

            var model = Serializer.SerializeModel(typeof(WritablePropertyTwin));

            JsonAssert.Equal(expectedModel, model);
        }

        [DigitalTwin]
        private class PropertyTwin : TwinBase
        {
            [TwinProperty] public int Counter { get; set; }
        }

        [DigitalTwin]
        private class SemanticPropertyTwin : TwinBase
        {
            [TwinProperty(SemanticType = SemanticType.Temperature, Unit = TemperatureUnit.DegreeCelsius)]
            public int Target { get; set; }
        }

        [DigitalTwin]
        private class TwinWithSchemaOverride : TwinBase
        {
            // Prior to dotnet6, there was no DateOnly framework type
            [TwinProperty(Schema = PrimitiveSchema.Date)] public string ManufacturedDateOnly { get; set; }
        }

        [DigitalTwin]
        private class WritablePropertyTwin : TwinBase
        {
            [TwinProperty(Writable = true)] public int Measurement { get; set; }
        }
    }
}
