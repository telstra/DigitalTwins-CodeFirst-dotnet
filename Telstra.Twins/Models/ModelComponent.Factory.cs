using System.Reflection;
using Telstra.Twins.Helpers;
using Telstra.Twins.Common;
using Telstra.Twins.Attributes;

namespace Telstra.Twins.Models
{
    public partial class ModelComponent : Content
    {
        public static ModelComponent Create(PropertyInfo info)
        {
            var t = info.PropertyType;
            var attr = info.GetCustomAttribute<TwinComponentAttribute>();

            return new ModelComponent()
            {
                Name = attr.Name?.ToCamelCase() ?? t.Name.ToCamelCase(),
                Schema = t.GetDigitalTwinModelId()
            };
        }
    }
}
