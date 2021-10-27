using System;
using System.Collections.Generic;
using System.Reflection;
using Telstra.Twins.Attributes;
using Telstra.Twins.Common;

namespace Telstra.Twins.Models
{
    public partial class ModelProperty : Content
    {
        public static ModelProperty Create(PropertyInfo info)
        {
            var property = new ModelProperty
            {
                Name = info.Name.ToCamelCase(),
                Schema = SchemaFromType(info.PropertyType)
            };

            if (Attribute.IsDefined(info, typeof(TwinTelemetryAttribute)))
            {
                var attr = info.GetCustomAttribute<TwinTelemetryAttribute>();
                if (attr != null)
                {
                    property.Type = "Telemetry";
                    property.SemanticType = attr.SemanticType;
                    property.Unit = attr.Unit;
                };
            }

            return property;
        }

        private static Dictionary<Type, string> SchemaMap = new Dictionary<Type, string>
        {
            { typeof(string), "string" },
            { typeof(bool), "boolean" },
            { typeof(bool?), "boolean" },
            { typeof(double), "double" },
            { typeof(double?), "double" },
            { typeof(int), "integer" },
            { typeof(int?), "integer" },
            {typeof(Int64), "integer" }
        };

        internal static string SchemaFromType(Type propertyType)
        {
            if (propertyType.IsGenericType)
                propertyType = propertyType.GetGenericArguments()[0];
            return SchemaMap.ContainsKey(propertyType) ? SchemaMap[propertyType] : null;
        }
    }
}
