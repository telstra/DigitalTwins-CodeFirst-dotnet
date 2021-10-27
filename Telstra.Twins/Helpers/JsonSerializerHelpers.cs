using Newtonsoft.Json;
using System.Text.Json;

namespace Telstra.Twins.Helpers
{
    public static class JsonSerializerHelpers
    {
        public static object ToObject(this JsonElement element)
        {
            var json = element.GetRawText();
            return JsonConvert.DeserializeObject(json);
        }
    }
}
