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
            return new ModelComponent(
                t.Name.ToCamelCase(),
                t.GetDigitalTwinModelId(),
                null,
                null,
                null,
                null
            );
        }
    }
}
