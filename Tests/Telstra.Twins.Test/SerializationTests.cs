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
        public void ShouldSerialiseModelToDTDL(string model, Type twinType)
        {
            var expectedModel = Serializer.SerializeModel(twinType);
            JsonAssert.Equals(model, expectedModel);
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
                DataGenerator.TwinWithNestedObjectModel,
                DataGenerator.twinWithNestedObject.GetType()
            };
        }

    }
}
