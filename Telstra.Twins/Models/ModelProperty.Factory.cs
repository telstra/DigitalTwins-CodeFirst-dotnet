using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json.Serialization;
using Telstra.Twins.Attributes;
using Telstra.Twins.Common;

namespace Telstra.Twins.Models
{
    public partial class ModelProperty
    {
        private static readonly Dictionary<Type, string> SchemaMap = new Dictionary<Type, string>
        {
            { typeof(string), PrimitiveSchema.String },
            { typeof(bool), PrimitiveSchema.Boolean },
            { typeof(bool?), PrimitiveSchema.Boolean },
            { typeof(double), PrimitiveSchema.Double },
            { typeof(double?), PrimitiveSchema.Double },
            { typeof(int), PrimitiveSchema.Integer },
            { typeof(int?), PrimitiveSchema.Integer },
            { typeof(Int64), PrimitiveSchema.Long },
            { typeof(DateTimeOffset), PrimitiveSchema.DateTime },
            { typeof(DateTimeOffset?), PrimitiveSchema.DateTime },
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
                propertyAttribute.Writable ? (bool?)true : null
            );

            return property;
        }

        internal class NestedField
        {
            public NestedField(string name, object schema)
            {
                this.name = name;
                this.schema = schema;
            }

            public string name { get; set; }
            public object schema { get; set; }
        }

        internal class EnumValue
        {
            public EnumValue(string name, string displayName, int value)
            {
                Name = name;
                DisplayName = displayName;
                Value = value;
            }

            [JsonPropertyName("name")]
            public string Name { get; init; }

            [JsonPropertyName("displayName")]
            public string DisplayName { get; init; }

            [JsonPropertyName("enumValue")]
            public int Value { get; init; }
        }

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
                var mapKey = new NestedField("name", PrimitiveSchema.String);
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
                        SchemaFromType(fieldInfo)));
                }

                schema.Add("fields", fields);
                return schema;
            }

            if (propertyType.IsEnum || (nullableType != null && nullableType.IsEnum))
            {
                var schema = new Dictionary<string, Object>();
                schema.Add("@type", PrimitiveSchema.Enum);
                schema.Add("valueSchema", PrimitiveSchema.Integer);
                var enumValues = new List<EnumValue>();
                foreach (var enumVal in Enum.GetValues(nullableType ?? propertyType))
                {
                    enumValues.Add(new EnumValue(enumVal.ToString(), enumVal.ToString(), (int)enumVal));
                }

                schema.Add("enumValues", enumValues);
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
