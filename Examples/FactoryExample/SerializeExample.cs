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

            var factory = Program.CreateFactoryTwin();

            var twin1Dtdl = serializer.SerializeTwin(factory);
            Console.WriteLine(twin1Dtdl);
            Console.WriteLine();

            var twin2Dtdl = serializer.SerializeTwin(factory.Floors[0].RunsLines[0].RunsSteps[0]);
            Console.WriteLine(twin2Dtdl);
            Console.WriteLine();
        }
    }
}
