using Telstra.Twins.Attributes;
using Telstra.Twins.Core;
using Telstra.Twins.Services;
using Xunit;

namespace Telstra.Twins.Test
{
    public class TwinPropertyAttributeTests
    {
        public static string TwinWithSchemaOverrideModel =
            "    {\r\n  \"@id\": \"dtmi:telstra:twins:test:twinwithschemaoverride;1\",\r\n  \"@type\": \"Interface\",\r\n  \"@context\": \"dtmi:dtdl:context;2\",\r\n  \"displayName\": \"Twin with schema override\",\r\n  \"contents\": [\r\n    {\r\n      \"@type\": \"Property\",\r\n      \"name\": \"manufacturedDateOnly\",\r\n      \"schema\": \"date\"\r\n    }\r\n  ]\r\n}";

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
            var model = Serializer.SerializeModel(typeof(TwinWithSchemaOverride));

            JsonAssert.Equal(TwinWithSchemaOverrideModel, model);
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

        [DigitalTwin(Version = 1, DisplayName = "Twin with schema override")]
        public class TwinWithSchemaOverride : TwinBase
        {
            // Prior to dotnet6, there was no DateOnly framework type
            [TwinProperty(Schema = "date")] public string ManufacturedDateOnly { get; set; }
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
            [TwinProperty(SemanticType = "Temperature", Unit = "degreeCelsius")]
            public int Target { get; set; }
        }

        [DigitalTwin]
        private class WritablePropertyTwin : TwinBase
        {
            [TwinProperty(Writable = true)] public int Measurement { get; set; }
        }
    }
}
