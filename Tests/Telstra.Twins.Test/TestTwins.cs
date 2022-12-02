using System;
using System.Collections.Generic;
using Telstra.Twins.Attributes;

namespace Telstra.Twins.Test
{
    public abstract class Space : TwinBase
    {
        [TwinProperty]
        public string Name { get; set; }
    }

    public abstract class BuildingBaseWithAbstractProperty : TwinBase
    {
        [TwinProperty]
        public abstract string AbstractProperty1 { get; }
    }

    [DigitalTwin(Version = 1, DisplayName = "BuildingWithAbstractProperty")]
    public class BuildingWithAbstractProperty : BuildingBaseWithAbstractProperty
    {
        [TwinProperty]
        public virtual string VirtualProperty1 { get; set; }

        [TwinProperty]
        public override string AbstractProperty1 { get; } = "Test";
    }

    [DigitalTwin(Version = 1, DisplayName = "BuildingWithAbstractProperty")]
    public class ExtendedBuildingWithAbstractParent : BuildingWithAbstractProperty
    {
        [TwinProperty]
        public override string VirtualProperty1 { get; set; } = "Default Value";

        [TwinProperty]
        public override string AbstractProperty1 { get; } = "Test";
    }

    [DigitalTwin(Version = 1, DisplayName = "Building")]
    public class Building : Space
    {
        [TwinProperty]
        public DateTimeOffset ConstructionDate { get; set; }

        [TwinProperty]
        public int? RoomsCount { get; set; }

        [TwinRelationship(Name = "contains")]
        public List<Floor> Floors { get; set; }

        [TwinRelationship(Name = "otherPlaces")]
        public List<Space> OtherSpaces { get; set; }
    }

    [DigitalTwin(Version = 1, DisplayName = "Extended Building")]
    public class ExtendedBuilding : Building
    {
        [TwinRelationship(Name = "hasTwins")]
        public List<SimpleTwin> SimpleTwins { get; set; }
    }

    [DigitalTwin(Version = 1, DisplayName = "Extended Building")]
    public class DoubleExtendedBuilding : ExtendedBuilding
    {
        [TwinProperty]
        public int Dummy { get; set; }
    }

    [DigitalTwin(Version = 1, DisplayName = "Floor")]
    public class Floor : Space
    {
        [TwinProperty]
        public decimal? Temperator { get; set; }
    }

    [DigitalTwin(Version = 1, DisplayName = "Simple Twin")]
    public class SimpleTwin : TwinBase
    {
        [TwinProperty]
        public int Quantity { get; set; }

        [TwinTelemetry]
        public int Measurement { get; set; }
    }

    [DigitalTwin(Version = 1, DisplayName = "Twin with a relationship")]
    public class TwinWithRelationship : TwinBase
    {

        [TwinProperty]
        public int Count { get; set; }

        [TwinRelationship(Name = "TestRelationship", MaxMultiplicity = 5)]
        public SimpleTwin Relationship { get; set; }
    }

    [DigitalTwin(Version = 1, DisplayName = "Twin with min multiplicity")]
    public class TwinWithMinMultiplicity : TwinBase
    {
        [TwinProperty]
        public int Count { get; set; }

        [TwinRelationship(Name = "Test Relationship", MinMultiplicity = 2)]
        public SimpleTwin Relationship { get; set; }
    }

    [DigitalTwin(Version = 1, DisplayName = "Twin with a nested object")]
    public class TwinWithNestedObject : TwinBase
    {
        public TwinWithNestedObject()
        { }
        [TwinProperty]
        public NestedObject NestedObj { get; set; }

        [TwinTelemetry]
        public int Speed { get; set; }
    }

    public class NestedObject
    {
        public NestedObject()
        { }

        public string Name { get; set; }
        public string Value { get; set; }
        public State State { get; set; }
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

        [TwinRelationship(MinMultiplicity = 1, MaxMultiplicity = 2)]
        public TwinWithNestedObject TwinRelationship { get; set; }

        [TwinProperty]
        public List<int> IntArray { get; set; }

        [TwinProperty]
        public Dictionary<string, string> StringMap { get; set; }

        [TwinProperty]
        public Guid Id { get; set; }

        [TwinProperty]
        public Guid? NullableId { get; set; }
    }

    public enum State
    {
        Actice = 1,
        Inactive = 2
    }

    [DigitalTwin(DisplayName = "Twin with enum property")]
    public class TwinWithEnum : TwinBase
    {
        [TwinProperty]
        public State State { get; set; }

        [TwinProperty]
        public State? NullableState { get; set; }
    }


    [DigitalTwin(DisplayName = "Twin with protected constructor")]
    public class TwinWithProtectedCtor : TwinBase
    {
        protected TwinWithProtectedCtor() { }

        public TwinWithProtectedCtor(string dummy) => Dummy = dummy;

        [TwinProperty]
        public string Dummy { get; set; }
    }

    [DigitalTwin(DisplayName = "Twin with protected constructor")]
    public class TwinWithReadOnlyProperties : TwinBase
    {
        [TwinProperty]
        public string Dummy { get; set; }

        [TwinProperty]
        public string ReaOnlyDummy => "DummyValue";
    }

    public static class DataGenerator
    {
        public static TwinWithNestedObject twinWithNestedObject = new TwinWithNestedObject()
        {
            TwinId = "11111",
            ETag = "abcd",
            NestedObj = new NestedObject()
            {
                Name = "name",
                Value = "value",
                State = State.Inactive
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
            ComponentTwin = new SimpleTwin() { Quantity = 1, Measurement = 2 },
            IntArray = new List<int>() { 1, 2, 3 },
            StringMap = new Dictionary<string, string>()
            {
                {"key","value"}
            },
            Id = Guid.Parse("b2d1ab5e-d953-4003-85e1-1018a00fe848"),
            NullableId = null
        };

        public static TwinWithEnum twinWithEnum = new TwinWithEnum()
        {
            TwinId = "123",
            ETag = "0",
            State = State.Inactive,
            NullableState = State.Actice
        };

        public static TwinWithReadOnlyProperties twinWithReadOnlyProperties = new TwinWithReadOnlyProperties()
        {
            TwinId = "123",
            ETag = "0",
            Dummy = "Test"
        };

        public static string SimpleTwinDTDL = "{\r\n  \"$dtId\": \"122233\",\r\n  \"$etag\": \"4444\",\r\n  \"$metadata\": {\r\n    \"$model\": \"dtmi:telstra:twins:test:simpletwin;1\",\r\n    \"propertyMetadata\": {}\r\n  },\r\n  \"measurement\": 2,\r\n  \"quantity\": 1\r\n}";
        public static string SimpleTwinModel = "    {\r\n  \"@id\": \"dtmi:telstra:twins:test:simpletwin;1\",\r\n  \"@type\": \"Interface\",\r\n  \"@context\": \"dtmi:dtdl:context;2\",\r\n  \"displayName\": \"Simple Twin\",\r\n  \"contents\": [\r\n    {\r\n      \"@type\": \"Property\",\r\n      \"name\": \"quantity\",\r\n      \"schema\": \"integer\"\r\n    },\r\n    {\r\n      \"@type\": \"Telemetry\",\r\n      \"name\": \"measurement\",\r\n      \"schema\": \"integer\"\r\n    }\r\n  ]\r\n}";
        public static string TwinWithNestedObjectDTDL = "{\r\n  \"$dtId\": \"11111\",\r\n  \"$etag\": \"abcd\",\r\n  \"$metadata\": {\r\n    \"$model\": \"dtmi:telstra:twins:test:twinwithnestedobject;1\",\r\n    \"propertyMetadata\": {}\r\n  },\r\n  \"speed\": 50,\r\n  \"nestedObj\": {\r\n    \"name\": \"name\",\r\n    \"value\": \"value\",\r\n    \"state\": \"Inactive\"\r\n  }\r\n}";
        public static string TwinWithNestedObjectModel = "{\r\n  \"@id\": \"dtmi:telstra:twins:test:twinwithnestedobject;1\",\r\n  \"@type\": \"Interface\",\r\n  \"@context\": \"dtmi:dtdl:context;2\",\r\n  \"displayName\": \"Twin with a nested object\",\r\n  \"contents\": [\r\n    {\r\n      \"schema\": {\r\n        \"@type\": \"Object\",\r\n        \"fields\": [\r\n          {\r\n            \"name\": \"name\",\r\n            \"schema\": \"string\"\r\n          },\r\n          {\r\n            \"name\": \"value\",\r\n            \"schema\": \"string\"\r\n          },\r\n          {\r\n            \"name\": \"state\",\r\n            \"schema\": {\r\n              \"@type\": \"Enum\",\r\n              \"valueSchema\": \"integer\",\r\n              \"enumValues\": [\r\n                {\r\n                  \"name\": \"Actice\",\r\n                  \"displayName\": \"Actice\",\r\n                  \"enumValue\": 1\r\n                },\r\n                {\r\n                  \"name\": \"Inactive\",\r\n                  \"displayName\": \"Inactive\",\r\n                  \"enumValue\": 2\r\n                }\r\n              ]\r\n            }\r\n          }\r\n        ]\r\n      },\r\n      \"@type\": \"Property\",\r\n      \"name\": \"nestedObj\"\r\n    },\r\n    {\r\n      \"schema\": \"integer\",\r\n      \"@type\": \"Telemetry\",\r\n      \"name\": \"speed\"\r\n    }\r\n  ]\r\n}";
        public static string TwinWithRelationshipDTDL = "    {\r\n  \"$dtId\": \"1221\",\r\n  \"$etag\": \"Etag\",\r\n  \"$metadata\": {\r\n    \"$model\": \"dtmi:telstra:twins:test:twinwithrelationship;1\",\r\n    \"propertyMetadata\": {}\r\n  },\r\n  \"count\": 10\r\n}";
        public static string TwinWithRelationshipModel = "{\r\n  \"@id\": \"dtmi:telstra:twins:test:twinwithrelationship;1\",\r\n  \"@type\": \"Interface\",\r\n  \"@context\": \"dtmi:dtdl:context;2\",\r\n  \"displayName\": \"Twin with a relationship\",\r\n  \"contents\": [\r\n    {\r\n      \"@type\": \"Property\",\r\n      \"name\": \"count\",\r\n      \"schema\": \"integer\"\r\n    },\r\n    {\r\n      \"maxMultiplicity\": 5,\r\n      \"target\": \"dtmi:telstra:twins:test:simpletwin;1\",\r\n      \"@type\": \"Relationship\",\r\n      \"name\": \"testRelationship\"\r\n    }\r\n  ]\r\n}";
        public static string TwinWithMinMultiplicityModel = "{\r\n  \"@id\": \"dtmi:telstra:twins:test:twinwithminmultiplicity;1\",\r\n  \"@type\": \"Interface\",\r\n  \"@context\": \"dtmi:dtdl:context;2\",\r\n  \"displayName\": \"Twin with min multiplicity\",\r\n  \"contents\": [\r\n    {\r\n      \"@type\": \"Property\",\r\n      \"name\": \"count\",\r\n      \"schema\": \"integer\"\r\n    },\r\n    {\r\n      \"minMultiplicity\": 2,\r\n      \"target\": \"dtmi:telstra:twins:test:simpletwin;1\",\r\n      \"@type\": \"Relationship\",\r\n      \"name\": \"test Relationship\"\r\n    }\r\n  ]\r\n}";
        public static string TwinWithAllAttributesDTDL = "{\r\n  \"$dtId\": \"0\",\r\n  \"$etag\": \"0\",\r\n  \"$metadata\": {\r\n    \"$model\": \"dtmi:telstra:twins:test:twinwithallattributes;1\",\r\n    \"propertyMetadata\": {}\r\n  },\r\n  \"flag\": true,\r\n  \"testProperty\": \"property\",\r\n  \"intArray\": [\r\n    1,\r\n    2,\r\n    3\r\n  ],\r\n  \"stringMap\": {\r\n    \"key\": \"value\"\r\n  },\r\n  \"id\": \"b2d1ab5e-d953-4003-85e1-1018a00fe848\",\r\n  \"nullableId\": null,\r\n  \"componentTwin\": {\r\n    \"measurement\": 2,\r\n    \"quantity\": 1\r\n  }\r\n}";
        public static string TwinWithAllAttributesModel = "{\r\n  \"@id\": \"dtmi:telstra:twins:test:twinwithallattributes;1\",\r\n  \"@type\": \"Interface\",\r\n  \"@context\": \"dtmi:dtdl:context;2\",\r\n  \"displayName\": \"Twin with all attributes\",\r\n  \"contents\": [\r\n    {\r\n      \"schema\": \"string\",\r\n      \"@type\": \"Property\",\r\n      \"name\": \"property\"\r\n    },\r\n    {\r\n      \"schema\": {\r\n        \"@type\": \"Array\",\r\n        \"elementSchema\": \"integer\"\r\n      },\r\n      \"@type\": \"Property\",\r\n      \"name\": \"intArray\"\r\n    },\r\n    {\r\n      \"schema\": {\r\n        \"@type\": \"Map\",\r\n        \"mapKey\": {\r\n          \"name\": \"name\",\r\n          \"schema\": \"string\"\r\n        },\r\n        \"mapValue\": {\r\n          \"name\": \"name\",\r\n          \"schema\": \"string\"\r\n        }\r\n      },\r\n      \"@type\": \"Property\",\r\n      \"name\": \"stringMap\"\r\n    },\r\n    {\r\n      \"schema\": \"string\",\r\n      \"@type\": \"Property\",\r\n      \"name\": \"id\"\r\n    },\r\n    {\r\n      \"schema\": \"string\",\r\n      \"@type\": \"Property\",\r\n      \"name\": \"nullableId\"\r\n    },\r\n    {\r\n      \"maxMultiplicity\": 2,\r\n      \"minMultiplicity\": 1,\r\n      \"target\": \"dtmi:telstra:twins:test:twinwithnestedobject;1\",\r\n      \"name\": \"twinRelationship\",\r\n      \"@type\": \"Relationship\"\r\n    },\r\n    {\r\n      \"schema\": \"dtmi:telstra:twins:test:simpletwin;1\",\r\n      \"name\": \"simpleTwin\",\r\n      \"@type\": \"Component\"\r\n    },\r\n    {\r\n      \"schema\": \"boolean\",\r\n      \"@type\": \"Telemetry\",\r\n      \"name\": \"flag\"\r\n    }\r\n  ]\r\n}";
        public static string TwinWithEnumDTDL2 = "{\r\n  \"$dtId\": \"123\",\r\n  \"$etag\": \"0\",\r\n  \"$metadata\": {\r\n    \"$model\": \"dtmi:telstra:twins:test:twinwithenum;1\",\r\n    \"propertyMetadata\": {}\r\n  },\r\n  \"state\": 2,\r\n  \"nullableState\": 1\r\n}";
        public static string TwinWithEnumDTDL = "{\r\n  \"$dtId\": \"123\",\r\n  \"$etag\": \"0\",\r\n  \"$metadata\": {\r\n    \"$model\": \"dtmi:telstra:twins:test:twinwithenum;1\",\r\n    \"propertyMetadata\": {}\r\n  },\r\n  \"state\": \"Inactive\",\r\n  \"nullableState\": \"Actice\"\r\n}";
        public static string TwinWithEnumModel = "{\r\n  \"@id\": \"dtmi:telstra:twins:test:twinwithenum;1\",\r\n  \"@type\": \"Interface\",\r\n  \"@context\": \"dtmi:dtdl:context;2\",\r\n  \"displayName\": \"Twin with enum property\",\r\n  \"contents\": [\r\n    {\r\n      \"schema\": {\r\n        \"@type\": \"Enum\",\r\n        \"valueSchema\": \"integer\",\r\n        \"enumValues\": [\r\n          {\r\n            \"name\": \"Actice\",\r\n            \"displayName\": \"Actice\",\r\n            \"enumValue\": 1\r\n          },\r\n          {\r\n            \"name\": \"Inactive\",\r\n            \"displayName\": \"Inactive\",\r\n            \"enumValue\": 2\r\n          }\r\n        ]\r\n      },\r\n      \"@type\": \"Property\",\r\n      \"name\": \"state\"\r\n    },\r\n    {\r\n      \"schema\": {\r\n        \"@type\": \"Enum\",\r\n        \"valueSchema\": \"integer\",\r\n        \"enumValues\": [\r\n          {\r\n            \"name\": \"Actice\",\r\n            \"displayName\": \"Actice\",\r\n            \"enumValue\": 1\r\n          },\r\n          {\r\n            \"name\": \"Inactive\",\r\n            \"displayName\": \"Inactive\",\r\n            \"enumValue\": 2\r\n          }\r\n        ]\r\n      },\r\n      \"@type\": \"Property\",\r\n      \"name\": \"nullableState\"\r\n    }\r\n  ]\r\n}";
        public static string TwinWithReadOnlyPropertiesDTDL = "{\r\n  \"$dtId\": \"123\",\r\n  \"$etag\": \"0\",\r\n  \"$metadata\": {\r\n    \"$model\": \"dtmi:telstra:twins:test:twinwithreadonlyproperties;1\",\r\n    \"propertyMetadata\": {}\r\n  },\r\n  \"dummy\": \"Test\",\r\n  \"reaOnlyDummy\": \"DummyValue\"\r\n}";
        public static string TwinWithReadOnlyPropertiesModel = "{\r\n  \"@id\": \"dtmi:telstra:twins:test:twinwithreadonlyproperties;1\",\r\n  \"@type\": \"Interface\",\r\n  \"@context\": \"dtmi:dtdl:context;2\",\r\n  \"displayName\": \"Twin with protected constructor\",\r\n  \"contents\": [\r\n    {\r\n      \"schema\": \"string\",\r\n      \"@type\": \"Property\",\r\n      \"name\": \"dummy\"\r\n    },\r\n    {\r\n      \"schema\": \"string\",\r\n      \"@type\": \"Property\",\r\n      \"name\": \"reaOnlyDummy\"\r\n    }\r\n  ]\r\n}";
    }
}
