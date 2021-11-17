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

            return new ModelRelationship(info.Name.ToCamelCase())
            {
                DisplayName = attr.DisplayName,
                Comment = attr.Comment,
                Description = attr.Description,
                MaxMultiplicity = attr.MaxMultiplicity == 0 ? null : (int?)attr.MaxMultiplicity,
                MinMultiplicity = attr.MinMultiplicity == 0 ? null : (int?)attr.MinMultiplicity,
                Target = info.PropertyType.GetModelPropertyType().GetDigitalTwinModelId()
            };
        }
    }
}
