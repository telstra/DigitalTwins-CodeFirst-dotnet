using System;
using Telstra.Twins.Attributes;
using Telstra.Twins.Core;
using Telstra.Twins.Services;
using Xunit;

namespace Telstra.Twins.Test
{
    public class TwinPropertyPrimitiveSchemaTests
    {
        public TwinPropertyPrimitiveSchemaTests()
        {
            var modelLibrary = new ModelLibrary();
            Serializer = new DigitalTwinSerializer(modelLibrary);
        }

        private DigitalTwinSerializer Serializer { get; }

        [Fact]
        public void DateTimePropertyShouldSerialiseToModel()
        {
            var expectedModel = @"{
  ""@id"": ""dtmi:telstra:twins:test:twinwithdatetime;1"",
  ""@type"": ""Interface"",
  ""@context"": ""dtmi:dtdl:context;2"",
  ""contents"": [
    {
      ""@type"": ""Property"",
      ""name"": ""manufactured"",
      ""schema"": ""dateTime""
    }
  ]
}";

            var model = Serializer.SerializeModel(typeof(TwinWithDateTime));

            JsonAssert.Equal(expectedModel, model);
        }

        [Fact]
        public void DateTimePropertyShouldSerialiseToTwin()
        {
            var expectedDtdl = @"{
  ""$dtId"": ""1234"",
  ""$etag"": ""5678"",
  ""$metadata"": {
    ""$model"": ""dtmi:telstra:twins:test:twinwithdatetime;1"",
    ""propertyMetadata"": {}
  },
  ""manufactured"": ""2021-11-17T22:23:00+10:00""
}";

            var twinWithDateTime = new TwinWithDateTime
            {
                TwinId = "1234",
                ETag = "5678",
                Manufactured = new DateTimeOffset(2021, 11, 17, 22, 23, 0, TimeSpan.FromHours(10))
            };
            var model = Serializer.SerializeTwin(twinWithDateTime);

            JsonAssert.Equal(expectedDtdl, model);
        }

        [Fact]
        public void NullableDateTimePropertyShouldSerialiseToModel()
        {
            var expectedModel = @"{
  ""@id"": ""dtmi:telstra:twins:test:twinwithnullabledatetime;1"",
  ""@type"": ""Interface"",
  ""@context"": ""dtmi:dtdl:context;2"",
  ""contents"": [
    {
      ""@type"": ""Property"",            
      ""name"": ""manufactured"",
      ""schema"": ""dateTime""
    }
  ]
}";

            var model = Serializer.SerializeModel(typeof(TwinWithNullableDateTime));

            JsonAssert.Equal(expectedModel, model);
        }

        [DigitalTwin]
        public class TwinWithDateTime : TwinBase
        {
            [TwinProperty] public DateTimeOffset Manufactured { get; set; }
        }

        [DigitalTwin]
        public class TwinWithNullableDateTime : TwinBase
        {
            [TwinProperty] public DateTimeOffset? Manufactured { get; set; }
        }
    }
}
