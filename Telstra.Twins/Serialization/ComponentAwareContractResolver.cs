using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;
using Telstra.Twins.Models;

namespace Telstra.Twins.Serialization
{
    public class ComponentAwareContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var prop = base.CreateProperty(member, memberSerialization);
            if (prop.DeclaringType == typeof(TwinMetadata) && prop.PropertyName == "$model")
            {
                prop.ShouldSerialize = instance => !(instance as TwinMetadata).IsComponent;
            }

            if (prop.PropertyName == "$type")
                prop.ShouldSerialize = instance => false;

            return prop;
        }
    }
}
