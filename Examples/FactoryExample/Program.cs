using System;
using System.Threading;
using System.Threading.Tasks;
using FactoryExample.Devices;
using FactoryExample.Models;
using FactoryExample.Schema;
using Microsoft.Extensions.Configuration;

namespace FactoryExample
{
    public static class Program
    {
        public static readonly Type[] ModelTypes =
        {
            typeof(Factory), typeof(FactoryFloor), typeof(ProductionLine), typeof(ProductionStep),
            typeof(ProductionStepGrinding)
        };

        public static Factory CreateFactoryTwin()
        {
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
                ComfortIndex = 0.8, FloorId = "floor1", FloorName = "FactoryFloor", Temperature = 23
            };
            factoryFloor.RunsLines.Add(productionLine);

            var factory = new Factory
            {
                Country = "AU",
                FactoryId = "factory1",
                FactoryName = "Chocolate Factory",
                GeoLocation = new GeoCord { Latitude = -27.4705, Longitude = 153.026 },
                LastSupplyDate = new DateTimeOffset(2021, 11, 17, 18, 37, 0, TimeSpan.FromHours(10)),
                Tags = String.Empty,
                ZipCode = "4000"
            };
            factory.Floors.Add(factoryFloor);
            return factory;
        }

        public static async Task Main(string[] args)
        {
            Console.WriteLine("Digital Twin Code First Factory Example");

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();

            var show = configuration.GetValue<string>("serialize");
            if (!string.IsNullOrWhiteSpace(show))
            {
                switch (show.ToLowerInvariant())
                {
                    case "model":
                        SerializeExample.SerializeModels();
                        return;
                    case "twin":
                        SerializeExample.SerializeTwins();
                        return;
                }
            }

            var check = configuration.GetValue<string>("parse");
            if (!string.IsNullOrWhiteSpace(check))
            {
                switch (check.ToLowerInvariant())
                {
                    case "model":
                        await ParseExample.ParseModelsAsync(CancellationToken.None);
                        return;
                }
            }

            var create = configuration.GetValue<string>("create");
            if (!string.IsNullOrWhiteSpace(create))
            {
                var adtEndpoint = configuration.GetValue<string>("endpoint");
                switch (create.ToLowerInvariant())
                {
                    case "model":
                        await ParseExample.ParseModelsAsync(CancellationToken.None);
                        await CreateExample.CreateModelsAsync(adtEndpoint, CancellationToken.None);
                        return;
                    case "twin":
                        await CreateExample.CreateTwinsAsync(adtEndpoint, CancellationToken.None);
                        return;
                }
            }

            ShowHelp();
        }

        private static void ShowHelp()
        {
            Console.WriteLine(" --serialize model : shows serialized model examples");
            Console.WriteLine(" --serialize example : shows serialized twin examples");
            Console.WriteLine(" --parse model : parse and validate the example model");
            Console.WriteLine(" --create model --endpoint <dtEndpoint>: parse and upload the example model");
        }
    }
}
