using System;
using System.Collections.Generic;
using System.Reflection;
using Telstra.Twins.Attributes;
using Telstra.Twins.Common;

namespace Telstra.Twins.Models
{
    public partial class ModelProperty
    {
        private static readonly Dictionary<Type, string> SchemaMap = new Dictionary<Type, string>
        {
            { typeof(string), "string" },
            { typeof(bool), "boolean" },
            { typeof(bool?), "boolean" },
            { typeof(double), "double" },
            { typeof(double?), "double" },
            { typeof(int), "integer" },
            { typeof(int?), "integer" },
            { typeof(Int64), "integer" }
        };

        public static ModelProperty Create(PropertyInfo info)
        {
            if (Attribute.IsDefined(info, typeof(TwinTelemetryAttribute)))
            {
                var attr = info.GetCustomAttribute<TwinTelemetryAttribute>();

                var telemetry = new ModelProperty(
                    true,
                    info.Name.ToCamelCase(),
                    SchemaFromType(info),
                    null,
                    null,
                    null,
                    null,
                    attr!.SemanticType,
                    attr.Unit,
                    null
                );

                return telemetry;
            }

            var property = new ModelProperty(
                false,
                info.Name.ToCamelCase(),
                SchemaFromType(info),
                null,
                null,
                null,
                null,
                null,
                null,
                null
            );

            return property;
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

        internal static object SchemaFromType(PropertyInfo info)
        {
            var propertyType = info.PropertyType;
            if (SchemaMap.ContainsKey(propertyType))
            {
                return SchemaMap[propertyType];
            }

            if (propertyType.IsArray)
            {
                var schema = new Dictionary<string, string>();
                schema.Add("@type", "Array");
                var arrayType = SchemaMap.TryGetValue(propertyType);
                schema.Add("elementSchema", arrayType);
                return schema;
            }

            if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(List<>))
            {
                var schema = new Dictionary<string, string>();
                schema.Add("@type", "Array");
                var listType = SchemaMap.TryGetValue(propertyType.GetGenericArguments()[0]);
                schema.Add("elementSchema", listType);
                return schema;
            }

            if (propertyType.IsGenericType &&
                propertyType.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                var schema = new Dictionary<string, Object>();
                schema.Add("@type", "Map");
                //TODO Logic to fill up the name field which is metadata
                var mapKey = new NestedField("name", "string");
                schema.Add("mapKey", mapKey);
                var mapValue = new NestedField("name",
                    SchemaMap.TryGetValue(propertyType.GetGenericArguments()[1]));
                schema.Add("mapValue", mapValue);
                return schema;
            }

            if (propertyType.IsClass)
            {
                var schema = new Dictionary<string, object>();
                schema.Add("@type", "Object");
                var fields = new List<NestedField>();
                var fieldsInfo = propertyType.GetProperties();
                foreach (var fieldInfo in fieldsInfo)
                {
                    fields.Add(new NestedField(fieldInfo.Name,
                        SchemaMap.TryGetValue(fieldInfo.PropertyType)));
                }

                schema.Add("fields", fields);
                return schema;
            }

            return null;
        }
    }

    internal static class Extensions
    {
        public static T TryGetValue<K, T>(this Dictionary<K, T> dict, K key)
        {
            return dict.ContainsKey(key) ? dict[key] : default;
        }
    }
}
