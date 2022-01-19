using System.Reflection;
using Telstra.Twins.Attributes;
using Telstra.Twins.Common;
using Telstra.Twins.Helpers;

namespace Telstra.Twins.Models
{
    public partial class ModelRelationship
    {
        public static ModelRelationship Create(PropertyInfo info)
        {
            var attr = info.GetCustomAttribute<TwinRelationshipAttribute>();

            return new ModelRelationship(info.Name.ToCamelCase(),
                null,
                attr!.DisplayName,
                attr.Description,
                attr.Comment,
                attr.MaxMultiplicity == 0 ? null : (int?)attr.MaxMultiplicity,
                attr.MinMultiplicity == 0 ? null : (int?)attr.MinMultiplicity,
                info.PropertyType.GetModelPropertyType().GetDigitalTwinModelId(),
                null
            );
        }
    }
}
