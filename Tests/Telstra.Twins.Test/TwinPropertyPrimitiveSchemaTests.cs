using System;
using Telstra.Twins.Attributes;
using Telstra.Twins.Core;
using Telstra.Twins.Services;
using Xunit;

namespace Telstra.Twins.Test
{
    public class TwinPropertyPrimitiveSchemaTests
    {
        public static TwinWithDateTime twinWithDateTime = new TwinWithDateTime
        {
            TwinId = "1234",
            ETag = "5678",
            Manufactured = new DateTimeOffset(2021, 11, 17, 22, 23, 0, TimeSpan.FromHours(10))
        };

        public static string TwinWithDateTimeDTDL =
            "{\r\n  \"$dtId\": \"1234\",\r\n  \"$etag\": \"5678\",\r\n  \"$metadata\": {\r\n    \"$model\": \"dtmi:telstra:twins:test:twinwithdatetime;1\",\r\n    \"PropertyMetadata\": {}\r\n  },\r\n  \"manufactured\": \"2021-11-17T22:23:00+10:00\"\r\n}";

        public static string TwinWithDateTimeModel =
            "    {\r\n  \"@id\": \"dtmi:telstra:twins:test:twinwithdatetime;1\",\r\n  \"@type\": \"Interface\",\r\n  \"@context\": \"dtmi:dtdl:context;2\",\r\n  \"displayName\": \"Twin with dateTime\",\r\n  \"contents\": [\r\n    {\r\n      \"@type\": \"Property\",\r\n      \"name\": \"manufactured\",\r\n      \"schema\": \"dateTime\"\r\n    }\r\n  ]\r\n}";

        public static string TwinWithNullableDateTimeModel =
            "    {\r\n  \"@id\": \"dtmi:telstra:twins:test:twinwithnullabledatetime;1\",\r\n  \"@type\": \"Interface\",\r\n  \"@context\": \"dtmi:dtdl:context;2\",\r\n  \"displayName\": \"Twin with nullable dateTime\",\r\n  \"contents\": [\r\n    {\r\n      \"@type\": \"Property\",\r\n      \"name\": \"manufactured\",\r\n      \"schema\": \"dateTime\"\r\n    }\r\n  ]\r\n}";

        public TwinPropertyPrimitiveSchemaTests()
        {
            var modelLibrary = new ModelLibrary();
            Serializer = new DigitalTwinSerializer(modelLibrary);
        }

        private DigitalTwinSerializer Serializer { get; }

        [Fact]
        public void DateTimePropertyShouldSerialiseToModel()
        {
            var model = Serializer.SerializeModel(typeof(TwinWithDateTime));

            JsonAssert.Equal(TwinWithDateTimeModel, model);
        }

        [Fact]
        public void DateTimePropertyShouldSerialiseToTwin()
        {
            var model = Serializer.SerializeTwin(twinWithDateTime);

            JsonAssert.Equal(TwinWithDateTimeDTDL, model);
        }

        [Fact]
        public void NullableDateTimePropertyShouldSerialiseToModel()
        {
            var model = Serializer.SerializeModel(typeof(TwinWithNullableDateTime));

            JsonAssert.Equal(TwinWithNullableDateTimeModel, model);
        }

        [DigitalTwin(Version = 1, DisplayName = "Twin with dateTime")]
        public class TwinWithDateTime : TwinBase
        {
            [TwinProperty] public DateTimeOffset Manufactured { get; set; }
        }

        [DigitalTwin(Version = 1, DisplayName = "Twin with nullable dateTime")]
        public class TwinWithNullableDateTime : TwinBase
        {
            [TwinProperty] public DateTimeOffset? Manufactured { get; set; }
        }
    }
}
