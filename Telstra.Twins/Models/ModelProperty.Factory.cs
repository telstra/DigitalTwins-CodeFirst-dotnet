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
                Schema = SchemaFromType(info)
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
            { typeof(DateTime), "dateTime" },
            { typeof(DateTimeOffset), "dateTime" },
            { typeof(bool), "boolean" },
            { typeof(bool?), "boolean" },
            { typeof(double), "double" },
            { typeof(double?), "double" },
            { typeof(int), "integer" },
            { typeof(int?), "integer" },
            {typeof(Int64), "integer" }
        };

        internal static object SchemaFromType(PropertyInfo info)
        {
            var propertyType = info.PropertyType;
            var nullableType = Nullable.GetUnderlyingType(propertyType);
            if (SchemaMap.ContainsKey(propertyType))
                return SchemaMap[propertyType];
            else if (nullableType != null && SchemaMap.ContainsKey(nullableType))
            {
                return SchemaMap[nullableType];
            }
            else if (propertyType.IsArray)
            {
                var schema = new Dictionary<string, string>();
                schema.Add("@type", "Array");
                var arrayType = SchemaMap.TryGetValue<Type, string>(propertyType);
                schema.Add("elementSchema", arrayType);
                return schema;
            }
            else if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(List<>))
            {
                var schema = new Dictionary<string, string>();
                schema.Add("@type", "Array");
                var listType = SchemaMap.TryGetValue<Type, string>(propertyType.GetGenericArguments()[0]);
                schema.Add("elementSchema", listType);
                return schema;
            }
            else if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                var schema = new Dictionary<string, Object>();
                schema.Add("@type", "Map");
                //TODO Logic to fill up the name field which is metadata
                var mapKey = new NestedField("name", "string");
                schema.Add("mapKey", mapKey);
                var mapValue = new NestedField("name", SchemaMap.TryGetValue<Type, string>(propertyType.GetGenericArguments()[1]));
                schema.Add("mapValue", mapValue);
                return schema;
            }
            else if (propertyType.IsClass)
            {
                var schema = new Dictionary<string, object>();
                schema.Add("@type", "Object");
                var fields = new List<NestedField>();
                var fieldsInfo = propertyType.GetProperties();
                foreach (var fieldInfo in fieldsInfo)
                {
                    fields.Add(new NestedField(fieldInfo.Name, SchemaMap.TryGetValue<Type, string>(fieldInfo.PropertyType)));
                }
                schema.Add("fields", fields);
                return schema;
            }
            else return null;
        }

        internal class NestedField
        {
            public NestedField(string name, string schema)
            {
                this.name = name;
                this.schema = schema;
            }
            public string name { get; set; }
            public string schema { get; set; }
        }

    }

    internal static class Extensions
    {
        public static T TryGetValue<K, T>(this Dictionary<K, T> dict, K key)
        {
            return dict.ContainsKey(key) ? dict[key] : default(T);
        }
    }
}
