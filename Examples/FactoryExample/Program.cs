using System;
using System.Threading.Tasks;
using FactoryExample.Devices;
using FactoryExample.Models;
using Microsoft.Extensions.Configuration;

namespace FactoryExample
{
    internal class Program
    {
        public static Type[] ModelTypes =
        {
            typeof(Factory), typeof(FactoryFloor), typeof(ProductionLine), typeof(ProductionStep),
            typeof(ProductionStepGrinding)
        };

        private static async Task Main(string[] args)
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
                        await ParseExample.ParseModels();
                        return;
                }
            }

            var create = configuration.GetValue<string>("create");
            if (!string.IsNullOrWhiteSpace(create))
            {
                switch (create.ToLowerInvariant())
                {
                    case "model":
                        await ParseExample.ParseModels();
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
            Console.WriteLine(" --create model <connection>: parse and upload the example model");
        }
    }
}
