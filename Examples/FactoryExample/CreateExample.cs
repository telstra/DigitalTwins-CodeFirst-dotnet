using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Azure.DigitalTwins.Core;
using Azure.Identity;
using Telstra.Twins.Core;
using Telstra.Twins.Services;

namespace FactoryExample
{
    public static class CreateExample
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
                    Console.WriteLine(
                        $"{modelData.Id}: {modelData.LanguageDisplayNames.FirstOrDefault().Value}");
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

                await CreateTwinInstanceAsync(client, factory.FactoryId, serializer.SerializeTwin(factory),
                    cancellationToken);

                foreach (var floor in factory.Floors)
                {
                    await CreateTwinInstanceAsync(client, floor.FloorId, serializer.SerializeTwin(floor),
                        cancellationToken);
                    await CreateTwinRelationshipAsync(client, "floors", factory.FactoryId,
                        floor.FloorId, cancellationToken);

                    foreach (var line in floor.RunsLines)
                    {
                        await CreateTwinInstanceAsync(client, line.LineId, serializer.SerializeTwin(line),
                            cancellationToken);
                        await CreateTwinRelationshipAsync(client, "runsLines", floor.FloorId,
                            line.LineId, cancellationToken);

                        foreach (var step in line.RunsSteps)
                        {
                            await CreateTwinInstanceAsync(client, step.StepId, serializer.SerializeTwin(step),
                                cancellationToken);
                            await CreateTwinRelationshipAsync(client, "runsSteps", line.LineId,
                                step.StepId, cancellationToken);
                        }

                        foreach (var step in line.RunsSteps)
                        {
                            if (step.StepLink != null)
                            {
                                await CreateTwinRelationshipAsync(client, "stepLink", step.StepId,
                                    step.StepLink.StepId, cancellationToken);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CREATE FAILED: {ex.Message}");
            }
        }

        private static async Task CreateTwinInstanceAsync(DigitalTwinsClient client, string? id, string dtdl,
            CancellationToken cancellationToken)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var basicDigitalTwin = JsonSerializer.Deserialize<BasicDigitalTwin>(dtdl);
            var response =
                await client.CreateOrReplaceDigitalTwinAsync(id, basicDigitalTwin, null, cancellationToken);
            if (response?.Value != null)
            {
                Console.WriteLine("CREATE TWIN SUCCESS: Id={0}, ETag={1}", response.Value.Id,
                    response.Value.ETag);
            }
        }

        private static async Task CreateTwinRelationshipAsync(DigitalTwinsClient client,
            string relationshipName, string? sourceId, string? targetId, CancellationToken cancellationToken)
        {
            if (sourceId == null)
            {
                throw new ArgumentNullException(nameof(sourceId));
            }

            if (targetId == null)
            {
                throw new ArgumentNullException(nameof(targetId));
            }

            var relationship = new BasicRelationship
            {
                Id = $"{sourceId}_{targetId}",
                Name = relationshipName,
                SourceId = sourceId,
                TargetId = targetId
            };
            var response = await client.CreateOrReplaceRelationshipAsync(sourceId, relationship.Id,
                relationship, null, cancellationToken);
            Console.WriteLine("CREATE RELATIONSHIP SUCCESS: Id={0}, ETag={1}", response.Value.Id,
                response.Value.ETag);
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
            var tokenCredential = new DefaultAzureCredential(true);

            var client = new DigitalTwinsClient(
                new Uri(adtEndpoint),
                tokenCredential);

            return client;
        }
    }
}
