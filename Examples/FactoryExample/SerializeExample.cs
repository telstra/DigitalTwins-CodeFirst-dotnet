using System;
using FactoryExample.Devices;
using FactoryExample.Models;
using FactoryExample.Schema;
using Telstra.Twins.Core;
using Telstra.Twins.Services;

namespace FactoryExample
{
    public static class SerializeExample
    {
        public static void SerializeModels()
        {
            var modelLibrary = new ModelLibrary();
            var serializer = new DigitalTwinSerializer(modelLibrary);

            foreach (var modelType in Program.ModelTypes)
            {
                var modelDtdl = serializer.SerializeModel(modelType);
                Console.WriteLine(modelDtdl);
                Console.WriteLine();
            }
        }

        public static void SerializeTwins()
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
                LastSupplyDate = new DateTimeOffset(2021, 11, 17, 18, 37, 0, TimeSpan.FromHours(10))
            };
            factory.Floors.Add(factoryFloor);

            var twin1Dtdl = serializer.SerializeTwin(factory);
            Console.WriteLine(twin1Dtdl);
            Console.WriteLine();

            var twin2Dtdl = serializer.SerializeTwin(factory.Floors[0].RunsLines[0].RunsSteps[0]);
            Console.WriteLine(twin2Dtdl);
            Console.WriteLine();
        }
    }
}
