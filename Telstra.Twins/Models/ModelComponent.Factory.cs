using System.Reflection;
using Telstra.Twins.Helpers;
using Telstra.Twins.Common;

namespace Telstra.Twins.Models
{
    public partial class ModelComponent : Content
    {
        public static ModelComponent Create(PropertyInfo info)
        {
            var t = info.PropertyType;
            return new ModelComponent()
            {
                Name = t.Name.ToCamelCase(),
                Schema = t.GetDigitalTwinModelId()
            };
        }
    }
}
