using System;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using FactoryExample.Devices;
using FactoryExample.Models;
using FactoryExample.Schema;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Azure.DigitalTwins.Parser;
using Microsoft.Extensions.Configuration;
using Telstra.Twins.Core;
using Telstra.Twins.Services;

Console.WriteLine("Digital Twin Code First Factory Example");

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", true)
    .AddEnvironmentVariables()
    .AddCommandLine(args)
    .Build();

var show = configuration.GetValue<string>("show");
if (!string.IsNullOrWhiteSpace(show))
{
    switch (show.ToLowerInvariant())
    {
        case "model":
            var modelTypes = new[]
            {
                typeof(Factory), typeof(FactoryFloor), typeof(ProductionLine), typeof(ProductionStep),
                typeof(ProductionStepGrinding)
            };
            ShowModels(modelTypes);
            return;
        case "example":
            ShowExamples();
            return;
    }
}

var create = configuration.GetValue<string>("create");
if (!string.IsNullOrWhiteSpace(create))
{
    switch (create.ToLowerInvariant())
    {
        case "model":
            var modelTypes = new[]
            {
                typeof(Factory), typeof(FactoryFloor), typeof(ProductionLine), typeof(ProductionStep),
                typeof(ProductionStepGrinding)
            };
            await CreateModels(modelTypes);
            return;
    }
}

ShowHelp();

async Task CreateModels(Type[] modelTypes)
{
    var modelLibrary = new ModelLibrary();
    var serializer = new DigitalTwinSerializer(modelLibrary);

    var models = modelTypes.Select(x => serializer.SerializeModel(x));

    // Validate
    try
    {
        var parser = new ModelParser();
        var entityInfos = await parser.ParseAsync(models);
        Console.WriteLine($"PARSE RESULTS:");
        foreach (var kvp in entityInfos)
        {
            Console.WriteLine($"[{kvp.Key}] = {kvp.Value.EntityKind} {kvp.Value.Id} ({kvp.Value.DisplayName.FirstOrDefault().Value})");
        }
    }
    catch (ParsingException ex)
    {
        Console.WriteLine($"PARSE FAILED: {ex.Message}");
        Console.WriteLine($"ERRORS:");
        var count = 0;
        foreach (var error in ex.Errors)
        {
            Console.WriteLine($"{++count}. {error}");
        }
    }
    catch (RequestFailedException ex)
    {
        Console.WriteLine($"REQUEST FAILED: {ex.Message}");
    }
}

void ShowHelp()
{
    Console.WriteLine(" --show model : shows the serialized model");
    Console.WriteLine(" --show example : shows a serialized twin");
    Console.WriteLine(" --create model : validate and uploads the model");
}

void ShowModels(Type[] modelTypes)
{
    var modelLibrary = new ModelLibrary();
    var serializer = new DigitalTwinSerializer(modelLibrary);

    foreach (var modelType in modelTypes)
    {
        var modelDtdl = serializer.SerializeModel(modelType);
        Console.WriteLine(modelDtdl);
        Console.WriteLine();
    }
}

void ShowExamples()
{
    var modelLibrary = new ModelLibrary();
    var serializer = new DigitalTwinSerializer(modelLibrary);

    var productionStepGrinding = new ProductionStepGrinding
    {
        ChassisTemperature = 50,
        FinalStep = false,
        Force = 8.0,
        GrindingTime = 30,
        //OperationStatus = ProductionStepStatus.Online,
        PowerUsage = 100,
        //StartTime = new DateTimeOffset(2021, 11, 17, 19, 57, 0, TimeSpan.FromHours(10)),
        StepId = "step1",
        StepName = "GrindingStep"
    };

    var productionLine = new ProductionLine
    {
        CurrentProductId = "production1",
        LineId = "line1",
        LineName = "ProductionLine",
        //LineOperationStatus = ProductionLineStatus.Online,
        ProductBatchNumber = 5
    };
    productionLine.RunsSteps.Add(productionStepGrinding);

    var factoryFloor = new FactoryFloor
    {
        FloorId = "floor1", FloorName = "FactoryFloor", Temperature = 23, ComfortIndex = 0.8
    };
    factoryFloor.RunsLines.Add(productionLine);

    var factory = new Factory
    {
        FactoryId = "factory1",
        Country = "AU",
        ZipCode = "4000",
        FactoryName = "Chocolate Factory",
        GeoLocation = new GeoCord { Latitude = -27.4705, Longitude = 153.026 },
        //LastSupplyDate = new DateTimeOffset(2021, 11, 17, 18, 37, 0, TimeSpan.FromHours(10))
    };
    factory.Floors.Add(factoryFloor);

    var twin1Dtdl = serializer.SerializeTwin(factory);
    Console.WriteLine(twin1Dtdl);
    Console.WriteLine();

    var twin2Dtdl = serializer.SerializeTwin(factory.Floors[0].RunsLines[0].RunsSteps[0]);
    Console.WriteLine(twin2Dtdl);
    Console.WriteLine();
}
