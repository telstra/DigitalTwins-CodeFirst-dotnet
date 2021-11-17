using System.Reflection;
using Telstra.Twins.Attributes;
using Telstra.Twins.Helpers;
using Telstra.Twins.Common;

namespace Telstra.Twins.Models
{
    public partial class ModelRelationship : Content
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
