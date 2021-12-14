using System;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Microsoft.Azure.DigitalTwins.Parser;
using Telstra.Twins.Core;
using Telstra.Twins.Services;

namespace FactoryExample
{
    public static class ParseExample
    {
        public static async Task ParseModels()
        {
            var modelLibrary = new ModelLibrary();
            var serializer = new DigitalTwinSerializer(modelLibrary);

            var models = Program.ModelTypes.Select(x => serializer.SerializeModel(x));

            try
            {
                var parser = new ModelParser();
                var entityInfos = await parser.ParseAsync(models);
                Console.WriteLine("PARSE SUCCESS:");
                foreach (var kvp in entityInfos)
                {
                    Console.WriteLine(
                        $"[{kvp.Key}] = {kvp.Value.EntityKind} {kvp.Value.Id} ({kvp.Value.DisplayName.FirstOrDefault().Value})");
                }
            }
            catch (ParsingException ex)
            {
                Console.WriteLine($"PARSE FAILED: {ex.Message}");
                Console.WriteLine("ERRORS:");
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
    }
}
