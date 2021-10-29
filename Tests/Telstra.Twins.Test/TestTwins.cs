using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using Telstra.Twins;
using Telstra.Twins.Attributes;
using Telstra.Twins.Models;
using Azure.DigitalTwins.Core;

namespace Telstra.Twins.Test
{
    [DigitalTwin(Version=1, DisplayName="Simple Twin")]
    public class SimpleTwin: TwinBase
    {
        [TwinProperty]
        public int Quantity { get; set; }
        [TwinTelemetry]
        public int Measurement { get; set; }
    }

    [DigitalTwin(Version=1,DisplayName="Twin with a relationship")]
    public class TwinWithRelationship: TwinBase
    {

        [TwinProperty]
        public int Count { get; set; }

        [TwinRelationship(Name = "Test Relationship", MaxMultiplicity = 5)]
        public SimpleTwin Relationship { get; set;} 

    }

    [DigitalTwin(Version=1,DisplayName="Twin with a nested object")]
    public class TwinWithNestedObject: TwinBase
    {
        public TwinWithNestedObject()
        {}
        [TwinProperty]
        public NestedObject NestedObj { get; set; }

        [TwinTelemetry]
        public int Speed { get; set; }
    }

    public class NestedObject
    {
        public NestedObject()
        {}

        public string name { get; set; }
        public string value { get; set; }

    }

    [DigitalTwin(DisplayName = "Twin with all attributes")]
    public class TwinWithAllAttributes : TwinBase
    {
        [TwinProperty("testProperty")]
        public string Property { get; set; }

        [TwinTelemetry]
        public bool Flag { get; set; }

        [TwinComponent]
        public SimpleTwin ComponentTwin { get; set; }

        [TwinRelationship(MaxMultiplicity = 1, MinMultiplicity = 2)]
        public TwinWithNestedObject TwinRelationship { get; set; }

        [TwinProperty]
        public List<int> IntArray { get; set; }

        [TwinProperty]
        public Dictionary<string, string> StringMap { get; set; }

}
    public static class DataGenerator
    {
        public static TwinWithNestedObject twinWithNestedObject = new TwinWithNestedObject()
        {
            TwinId = "11111",
            ETag = "abcd",
            NestedObj = new NestedObject()
            {
                name = "name",
                value = "value"
            },
            Speed = 50
        };

        public static TwinWithRelationship twinWithRelationship = new TwinWithRelationship()
        {
            TwinId = "1221",
            ETag = "Etag",
            Count = 10
        };

        public static SimpleTwin simpleTwin = new SimpleTwin()
        {
            TwinId = "122233",
            ETag = "4444",
            Quantity = 1,
            Measurement = 2
        };

        public static TwinWithAllAttributes twinWithAllAttributes = new TwinWithAllAttributes()
        {
            TwinId = "0",
            ETag = "0",
            Property = "property",
            Flag = true,
            ComponentTwin = simpleTwin,
            IntArray = new List<int>() {1 ,2, 3},
            StringMap = new Dictionary<string, string>() 
            {
                {"key","value"}
            }
        };

        public static string SimpleTwinDTDL = "";
        public static string SimpleTwinModel = "    {\r\n  \"@id\": \"dtmi:telstra:twins:test:simpletwin;1\",\r\n  \"@type\": \"Interface\",\r\n  \"@context\": \"dtmi:dtdl:context;2\",\r\n  \"displayName\": \"Simple Twin\",\r\n  \"contents\": [\r\n    {\r\n      \"@type\": \"Property\",\r\n      \"name\": \"quantity\",\r\n      \"schema\": \"integer\"\r\n    },\r\n    {\r\n      \"@type\": \"Telemetry\",\r\n      \"name\": \"measurement\",\r\n      \"schema\": \"integer\"\r\n    }\r\n  ]\r\n}";
        public static string TwinWithNestedObjectDTDL = "    {\r\n  \"$dtId\": \"11111\",\r\n  \"$etag\": \"abcd\",\r\n  \"$metadata\": {\r\n    \"$model\": \"dtmi:telstra:twins:test:twinwithnestedobject;1\",\r\n    \"PropertyMetadata\": {}\r\n  },\r\n  \"nestedObj\": {\r\n    \"name\": \"name\",\r\n    \"value\": \"value\"\r\n  }\r\n}";
        public static string TwinWithNestedObjectModel = "  {\r\n  \"@id\": \"dtmi:telstra:twins:test:twinwithnestedobject;1\",\r\n  \"@type\": \"Interface\",\r\n  \"@context\": \"dtmi:dtdl:context;2\",\r\n  \"displayName\": \"Twin with a nested object\",\r\n  \"contents\": [\r\n    {\r\n      \"@type\": \"Property\",\r\n      \"name\": \"nestedObj\",\r\n      \"schema\": {\r\n        \"@type\": \"Object\",\r\n        \"fields\": [\r\n          {\r\n            \"name\": \"name\",\r\n            \"schema\": \"string\"\r\n          },\r\n          {\r\n            \"name\": \"value\",\r\n            \"schema\": \"string\"\r\n          }\r\n        ]\r\n      }\r\n    },\r\n    {\r\n      \"@type\": \"Telemetry\",\r\n      \"name\": \"speed\",\r\n      \"schema\": \"integer\"\r\n    }\r\n  ]\r\n}";
        public static string TwinWithRelationshipDTDL = "    {\r\n  \"$dtId\": \"1221\",\r\n  \"$etag\": \"Etag\",\r\n  \"$metadata\": {\r\n    \"$model\": \"dtmi:telstra:twins:test:twinwithrelationship;1\",\r\n    \"PropertyMetadata\": {}\r\n  },\r\n  \"count\": 10\r\n}";
        public static string TwinWithRelationshipModel = "{\r\n  \"@id\": \"dtmi:telstra:twins:test:twinwithrelationship;1\",\r\n  \"@type\": \"Interface\",\r\n  \"@context\": \"dtmi:dtdl:context;2\",\r\n  \"displayName\": \"Twin with a relationship\",\r\n  \"contents\": [\r\n    {\r\n      \"@type\": \"Property\",\r\n      \"name\": \"count\",\r\n      \"schema\": \"integer\"\r\n    },\r\n    {\r\n      \"maxMultiplicity\": 5,\r\n      \"minMultiplicity\": 0,\r\n      \"target\": \"dtmi:telstra:twins:test:simpletwin;1\",\r\n      \"@type\": \"Relationship\",\r\n      \"name\": \"relationship\"\r\n    }\r\n  ]\r\n}";
        public static string TwinWithAllAttributesDTDL = "";
        public static string TwinWithAllAttributesModel = "    {\r\n  \"@id\": \"dtmi:telstra:twins:test:twinwithallattributes;1\",\r\n  \"@type\": \"Interface\",\r\n  \"@context\": \"dtmi:dtdl:context;2\",\r\n  \"displayName\": \"Twin with all attributes\",\r\n  \"contents\": [\r\n    {\r\n      \"@type\": \"Property\",\r\n      \"name\": \"property\",\r\n      \"schema\": \"string\"\r\n    },\r\n    {\r\n      \"@type\": \"Property\",\r\n      \"name\": \"intArray\",\r\n      \"schema\": {\r\n        \"@type\": \"Array\",\r\n        \"elementSchema\": \"integer\"\r\n      }\r\n    },\r\n    {\r\n      \"@type\": \"Property\",\r\n      \"name\": \"stringMap\",\r\n      \"schema\": {\r\n        \"@type\": \"Map\",\r\n        \"mapKey\": {\r\n          \"name\": \"name\",\r\n          \"schema\": \"string\"\r\n        },\r\n        \"mapValue\": {\r\n          \"name\": \"name\",\r\n          \"schema\": \"string\"\r\n        }\r\n      }\r\n    },\r\n    {\r\n      \"maxMultiplicity\": 1,\r\n      \"minMultiplicity\": 2,\r\n      \"target\": \"dtmi:telstra:twins:test:twinwithnestedobject;1\",\r\n      \"@type\": \"Relationship\",\r\n      \"name\": \"twinRelationship\"\r\n    },\r\n    {\r\n      \"@type\": \"Component\",\r\n      \"name\": \"simpleTwin\",\r\n      \"schema\": \"dtmi:telstra:twins:test:simpletwin;1\"\r\n    },\r\n    {\r\n      \"@type\": \"Telemetry\",\r\n      \"name\": \"flag\",\r\n      \"schema\": \"boolean\"\r\n    }\r\n  ]\r\n}";
    }
}
