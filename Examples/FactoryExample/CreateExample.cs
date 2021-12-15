using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.DigitalTwins.Core;
using Azure.Identity;
using Microsoft.Azure.DigitalTwins.Parser;
using Telstra.Twins.Core;
using Telstra.Twins.Services;

namespace FactoryExample
{
    public class CreateExample
    {
        //public static async Task CreateModels(string tenantId, string clientId, string clientSecret, string adtEndpoint)
        public static async Task CreateModelsAsync(string adtEndpoint, CancellationToken cancellationToken)
        {
            var modelLibrary = new ModelLibrary();
            var serializer = new DigitalTwinSerializer(modelLibrary);

            var models = Program.ModelTypes.Select(x => serializer.SerializeModel(x));

            try
            {
                //var client = GetDigitalTwinsClient(tenantId,  clientId,  clientSecret, adtEndpoint);
                var client = GetDigitalTwinsClient(adtEndpoint);
                var response = await client.CreateModelsAsync(models, cancellationToken);
                Console.WriteLine("CREATE MODELS SUCCESS");
                foreach (var modelData in response.Value)
                {
                    Console.WriteLine($"{modelData.Id}: {modelData.LanguageDisplayNames.FirstOrDefault().Value}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CREATE FAILED: {ex.Message}");
            }
        }

        public static async Task CreateTwinsAsync(string adtEndpoint, CancellationToken cancellationToken)
        {
            var modelLibrary = new ModelLibrary();
            var serializer = new DigitalTwinSerializer(modelLibrary);

            var factory = Program.CreateFactoryTwin();

            try
            {
                //var client = GetDigitalTwinsClient(tenantId,  clientId,  clientSecret, adtEndpoint);
                var client = GetDigitalTwinsClient(adtEndpoint);

                // Twin 1 
                var dtdl = serializer.SerializeTwin(factory);
                var basicDigitalTwin = JsonSerializer.Deserialize<BasicDigitalTwin>(dtdl);
                Response<BasicDigitalTwin> response = await client.CreateOrReplaceDigitalTwinAsync<BasicDigitalTwin>(factory.FactoryId, basicDigitalTwin, null, cancellationToken);
                Console.WriteLine("CREATE MODELS SUCCESS: Id={0}, ETag={1}", response.Value.Id, response.Value.ETag);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CREATE FAILED: {ex.Message}");
            }
        }

        private static DigitalTwinsClient GetDigitalTwinsClient(string adtEndpoint)
        //private static DigitalTwinsClient GetDigitalTwinsClient(string tenantId, string clientId, string clientSecret, string adtEndpoint)
        {
            // These environment variables are necessary for DefaultAzureCredential to use application Id and client secret to login.
            //Environment.SetEnvironmentVariable("AZURE_CLIENT_SECRET", clientSecret);
            //Environment.SetEnvironmentVariable("AZURE_CLIENT_ID", clientId);
            //Environment.SetEnvironmentVariable("AZURE_TENANT_ID", tenantId);

            // DefaultAzureCredential supports different authentication mechanisms and determines the appropriate credential type based of the environment it is executing in.
            // It attempts to use multiple credential types in an order until it finds a working credential.
            var tokenCredential = new DefaultAzureCredential(includeInteractiveCredentials: true);

            var client = new DigitalTwinsClient(
                new Uri(adtEndpoint),
                tokenCredential);

            return client;
        }
    }
}
