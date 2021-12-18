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
            typeof(ProductionStepGrinding), typeof(ProductionStepFanning), typeof(ProductionStepMoulding)
        };

        public static Factory CreateFactoryTwin()
        {
            var production1Step3Moulding = new ProductionStepMoulding
            {
                ChassisTemperature = 50,
                FinalStep = true,
                //OperationStatus = ProductionStepStatus.Online,
                PowerUsage = 100,
                StartTime = DateTimeOffset.UnixEpoch,
                StepId = "line1.step3",
                StepName = "Moulding Step"
            };
            
            var production1Step2Grinding = new ProductionStepGrinding
            {
                ChassisTemperature = 50,
                FinalStep = false,
                Force = 8.0,
                GrindingTime = 30,
                //OperationStatus = ProductionStepStatus.Online,
                PowerUsage = 100,
                StartTime = DateTimeOffset.UnixEpoch,
                StepId = "line1.step2",
                StepName = "Grinding Step",
                StepLink = production1Step3Moulding
            };

            var production1Step1Fanning = new ProductionStepFanning()
            {
                ChassisTemperature = 50,
                FinalStep = false,
                FanSpeed = 0.5,
                //OperationStatus = ProductionStepStatus.Online,
                PowerUsage = 100,
                StartTime = DateTimeOffset.UnixEpoch,
                StepId = "line1.step1",
                StepName = "Fanning Step",
                StepLink = production1Step2Grinding
            };

            var productionLine1 = new ProductionLine
            {
                CurrentProductId = "product5",
                LineId = "line1",
                LineName = "Production Line 1",
                //LineOperationStatus = ProductionLineStatus.Online,
                ProductBatchNumber = 6
            };
            productionLine1.RunsSteps.Add(production1Step1Fanning);
            productionLine1.RunsSteps.Add(production1Step2Grinding);
            productionLine1.RunsSteps.Add(production1Step3Moulding);

            var factory1Floor1 = new FactoryFloor
            {
                ComfortIndex = 0.8, FloorId = "factory1.floor1", FloorName = "Factory 1 Floor 1", Temperature = 23
            };
            factory1Floor1.RunsLines.Add(productionLine1);

            var factory1 = new Factory
            {
                Country = "AU",
                FactoryId = "factory1",
                FactoryName = "Chocolate Factory",
                GeoLocation = new GeoCord { Latitude = -27.4705, Longitude = 153.026 },
                LastSupplyDate = new DateTimeOffset(2021, 11, 17, 18, 37, 0, TimeSpan.FromHours(10)),
                Tags = String.Empty,
                ZipCode = "4000"
            };
            factory1.Floors.Add(factory1Floor1);
            return factory1;
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
