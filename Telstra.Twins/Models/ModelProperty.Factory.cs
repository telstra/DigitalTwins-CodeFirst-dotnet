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
            { typeof(Int64), "integer" },
            // Primitive type dateTime, which is date-time including offset from RFC 3339
            // https://github.com/Azure/opendigitaltwins-dtdl/blob/master/DTDL/v2/dtdlv2.md#primitive-schemas
            { typeof(DateTimeOffset), "dateTime" },
            { typeof(DateTimeOffset?), "dateTime" }
        };

        public static ModelProperty Create(PropertyInfo info)
        {
            if (Attribute.IsDefined(info, typeof(TwinTelemetryAttribute)))
            {
                var telemetryAttribute = info.GetCustomAttribute<TwinTelemetryAttribute>();

                var telemetry = new ModelProperty(
                    true,
                    info.Name.ToCamelCase(),
                    SchemaFromType(info),
                    null,
                    null,
                    null,
                    null,
                    telemetryAttribute!.SemanticType,
                    telemetryAttribute.Unit,
                    null
                );

                return telemetry;
            }

            var propertyAttribute = info.GetCustomAttribute<TwinPropertyAttribute>();

            var property = new ModelProperty(
                false,
                info.Name.ToCamelCase(),
                propertyAttribute!.Schema ?? SchemaFromType(info),
                null,
                null,
                null,
                null,
                propertyAttribute.SemanticType,
                propertyAttribute.Unit,
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
                var arrayType = SchemaMap.GetValueOrDefault(propertyType);
                schema.Add("elementSchema", arrayType);
                return schema;
            }

            if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(List<>))
            {
                var schema = new Dictionary<string, string>();
                schema.Add("@type", "Array");
                var listType = SchemaMap.GetValueOrDefault(propertyType.GetGenericArguments()[0]);
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
                    SchemaMap.GetValueOrDefault(propertyType.GetGenericArguments()[1]));
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
                        SchemaMap.GetValueOrDefault(fieldInfo.PropertyType)));
                }

                schema.Add("fields", fields);
                return schema;
            }

            return null;
        }
    }

    internal static class Extensions
    {
        public static TValue GetValueOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dictionary,
            TKey key)
        {
            return dictionary.TryGetValue(key, out TValue value) ? value : default;
        }
    }
}
