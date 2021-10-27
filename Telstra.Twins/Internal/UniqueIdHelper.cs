using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Azure;
using Azure.DigitalTwins.Core;

namespace Telstra.Twins.Internal
{
    internal static class UniqueIdHelper
    {
        private static readonly Random SRandom = new Random();

        internal static async Task<string> GetUniqueModelIdAsync(string baseName, DigitalTwinsClient client)
        {
            return await GetUniqueIdAsync(baseName, (modelId) => client.GetModelAsync(modelId));
        }

        internal static async Task<string> GetUniqueTwinIdAsync(string baseName, DigitalTwinsClient client)
        {
            return await GetUniqueIdAsync(baseName, async (twinId) => await client.GetDigitalTwinAsync<TwinBase>(twinId));
        }

        private static async Task<string> GetUniqueIdAsync(string baseName, Func<string, Task> getResource)
        {
            const int maxAttempts = 10;
            const int maxVal = 10000;
            var id = $"{baseName}{SRandom.Next(maxVal)}";

            for (var attemptsMade = 0; attemptsMade < maxAttempts; attemptsMade++)
            {
                try
                {
                    await getResource(id);
                }
                catch (RequestFailedException ex) when (ex.Status == (int)HttpStatusCode.NotFound)
                {
                    return id;
                }
                id = $"{baseName}{SRandom.Next(maxVal)}";
            }

            throw new Exception($"Unique Id could not be found with base {baseName}");
        }
    }
}
