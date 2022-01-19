#nullable enable

using System.Reflection;
using Telstra.Twins.Common;
using Telstra.Twins.Helpers;

namespace Telstra.Twins.Models
{
    public partial class ModelComponent
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
