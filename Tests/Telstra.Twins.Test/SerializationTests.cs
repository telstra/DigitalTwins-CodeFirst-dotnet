#nullable enable
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Azure.DigitalTwins.Core;
using Telstra.Twins.Attributes;
using Telstra.Twins.Core;
using Telstra.Twins.Models;
using Telstra.Twins.Services;
using Xunit;
using Xunit.Abstractions;
using FluentAssertions;

namespace Telstra.Twins.Test
{
    public class SerializationTests
    {
        protected ITestOutputHelper TestOutput { get; }
        protected DigitalTwinSerializer Serializer { get; }

        public SerializationTests(ITestOutputHelper testOutputHelper)
        {
            var modelLibrary = new ModelLibrary();
            Serializer = new DigitalTwinSerializer(modelLibrary);
            TestOutput = testOutputHelper;
        }

        [Theory]
        [MemberData(nameof(ModelTestData))]
        public void ShouldSerialiseModelToDTDL(string expectedModel, Type twinType)
        {
            var model = Serializer.SerializeModel(twinType);
            JsonAssert.Equal(expectedModel, model);

        }

        [Theory]
        [MemberData(nameof(TwinTestData))]
        public void ShouldSerialiseTwinToDTDL(string twinDTDL, object twinObject)
        {
            var expectedDTDL = Serializer.SerializeTwin(twinObject);
            JsonAssert.Equal(twinDTDL, expectedDTDL);
        }

        public static IEnumerable<object[]> ModelTestData()
        {
            yield return new object[] {
                DataGenerator.SimpleTwinModel,
                DataGenerator.simpleTwin.GetType()
            };
            yield return new object[] {
                DataGenerator.TwinWithAllAttributesModel,
                DataGenerator.twinWithAllAttributes.GetType()
            };
            yield return new object[] {
                DataGenerator.TwinWithNestedObjectModel,
                DataGenerator.twinWithNestedObject.GetType()
            };
            yield return new object[] {
                DataGenerator.TwinWithRelationshipModel,
                DataGenerator.twinWithRelationship.GetType()
            };
            yield return new object[] {
                DataGenerator.TwinWithMinMultiplicityModel,
                typeof(TwinWithMinMultiplicity)
            };
        }

        public static IEnumerable<object[]> TwinTestData()
        {
            yield return new object[] {
                DataGenerator.SimpleTwinDTDL,
                DataGenerator.simpleTwin
            };
            yield return new object[] {
                DataGenerator.TwinWithAllAttributesDTDL,
                DataGenerator.twinWithAllAttributes
            };
            yield return new object[] {
                DataGenerator.TwinWithNestedObjectDTDL,
                DataGenerator.twinWithNestedObject
            };
            yield return new object[] {
                DataGenerator.TwinWithRelationshipDTDL,
                DataGenerator.twinWithRelationship
            };
        }

    }
}
