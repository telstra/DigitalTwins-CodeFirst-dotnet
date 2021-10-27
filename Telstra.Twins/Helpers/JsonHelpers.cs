using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Telstra.Twins.Helpers
{
    public static class JsonHelpers
    {
        public static string GetModelId(string json) => JObject.Parse(json)["$metadata"]["$model"].ToString();

        public static string GetTwinId(string json) => JObject.Parse(json)["$dtId"].ToString();
    }
}
