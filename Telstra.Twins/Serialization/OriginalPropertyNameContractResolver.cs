using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;
using Telstra.Twins.Common;

namespace Telstra.Twins.Serialization
{
    public class OriginalPropertyNameContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var prop = base.CreateProperty(member, memberSerialization);

            prop.PropertyName = prop.UnderlyingName.ToCamelCase();

            return prop;
        }
    }
}
