#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Azure.DigitalTwins.Core;
using Telstra.Twins.Attributes;
using Telstra.Twins.Common;
using Telstra.Twins.Enums;
using Telstra.Twins.Helpers;

namespace Telstra.Twins.Serialization
{
    public class ObjectToTwinConverter<T> : TwinConverterBase<T>
    {
        public virtual string[] SpecialTwinPropertyNames => new[] { DigitalTwinsJsonPropertyNames.DigitalTwinId, DigitalTwinsJsonPropertyNames.DigitalTwinETag, DigitalTwinsJsonPropertyNames.DigitalTwinMetadata, "@context", "displayName" };

        public virtual string[] TwinPropertyNamesToExclude => new string[] { "@id", "@type", "extends", "@context", "displayName" };

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            //
            // First, write the reserved model properties
            //
            var specialTwinProperties = GetTwinOnlyProperties(SpecialTwinPropertyNames, TwinPropertyNamesToExclude)
                .ToNameValueDictionary(value, prop => prop.GetTwinPropertyName());

            foreach (var pair in specialTwinProperties)
            {
                var (propertyName, propertyValue) = pair;
                switch (propertyValue)
                {
                    case string castValue:
                        writer.WriteString(propertyName, castValue);
                        break;

                    case int castValue:
                        writer.WriteNumber(propertyName, castValue);
                        break;

                    case decimal castValue:
                        writer.WriteNumber(propertyName, castValue);
                        break;

                    case bool castValue:
                        writer.WriteBoolean(propertyName, castValue);
                        break;

                    default:
                        writer.WritePropertyName(propertyName);
                        JsonSerializer.Serialize(writer, propertyValue, options);
                        break;
                }
            }

            //
            // Next, write the normal properties of the twin
            //
            var normalTwinPropertiesDictionary = GetNormalTwinProperties(SpecialTwinPropertyNames, TwinPropertyNamesToExclude)
                .ToNameValueDictionary(value, prop => prop.GetTwinPropertyName());

            foreach (var pair in normalTwinPropertiesDictionary)
            {
                var (propertyName, propertyValue) = pair;
                switch (propertyValue)
                {
                    case string castValue:
                        writer.WriteString(propertyName, castValue);
                        break;

                    case int castValue:
                        writer.WriteNumber(propertyName, castValue);
                        break;

                    case decimal castValue:
                        writer.WriteNumber(propertyName, castValue);
                        break;

                    case bool castValue:
                        writer.WriteBoolean(propertyName, castValue);
                        break;

                    default:
                        writer.WritePropertyName(propertyName);
                        JsonSerializer.Serialize(writer, propertyValue, options);
                        break;
                }
            }

            //
            // Next, write the component properties of the twin
            //
            
            var componentPropertiesDictionary = GetComponentTwinProperties().ToNameValueDictionary(value, prop => prop.GetTwinPropertyName());

            foreach (var pair in componentPropertiesDictionary)
            {
                var (propertyName, propertyValue) = pair;
                writer.WritePropertyName(propertyName);
                if (propertyValue == null)
                {
                    writer.WriteNull(propertyName);
                }
                else
                {
                    // Create the custom converter
                    var twinComponentConverterType = typeof(ObjectToTwinConverter<>);
                    var concreteTwinComponentConverterType =
                        twinComponentConverterType.MakeGenericType(propertyValue.GetType());
                    var componentConverter = Activator.CreateInstance(concreteTwinComponentConverterType) as JsonConverter;

                    var componentSerializationOptions = new JsonSerializerOptions()
                    {
                        IgnoreNullValues = options.IgnoreNullValues,
                        Converters = { componentConverter! },
                        WriteIndented = options.WriteIndented
                    };

                    JsonSerializer.Serialize(writer, propertyValue, componentSerializationOptions);
                }
            }

            writer.WriteEndObject();
        }

        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var specialTwinProperties = GetTwinOnlyProperties(SpecialTwinPropertyNames);

            var twinInstance = Activator.CreateInstance(typeToConvert);
            if (twinInstance is T)
            {
                var normalTwinProperties =
                    GetNormalTwinProperties(SpecialTwinPropertyNames);

                var allTwinProperties = specialTwinProperties.Union(normalTwinProperties); //new List<PropertyInfo>(specialTwinProperties);
                allTwinProperties = allTwinProperties.Union(GetComponentTwinProperties());
                var propMap = allTwinProperties
                    .ToDictionary(
                        prop => prop.GetTwinPropertyName(),
                        prop => prop);

                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.PropertyName)
                    {
                        var name = reader.GetString();
                        if (!reader.Read())
                        {
                            throw new JsonException();
                        }

                        if (name != null)
                        {
                            // Does the Json property match one of the special model only properties?
                            if (propMap.TryGetValue(name, out var property))
                            {
                                // What type is the property?
                                var propertyType = property.PropertyType;

                                if (propertyType == typeof(string))
                                {
                                    var typedValue = reader.GetString();
                                    if (typedValue != null)
                                    {
                                        property.SetValue(twinInstance, typedValue);
                                    }
                                }
                                else if (propertyType == typeof(short))
                                {
                                    var typedValue = reader.GetInt16();
                                    property.SetValue(twinInstance, typedValue);
                                }
                                else if (propertyType == typeof(int))
                                {
                                    var typedValue = reader.GetInt32();
                                    property.SetValue(twinInstance, typedValue);
                                }
                                else if (propertyType == typeof(long))
                                {
                                    var typedValue = reader.GetInt64();
                                    property.SetValue(twinInstance, typedValue);
                                }
                                else if (propertyType == typeof(double))
                                {
                                    var typedValue = reader.GetDouble();
                                    property.SetValue(twinInstance, typedValue);
                                }
                                else if (propertyType == typeof(char))
                                {
                                    var typedValue = reader.GetString();
                                    if (typedValue != null)
                                    {
                                        property.SetValue(twinInstance, typedValue?.First() ?? '\0');
                                    }
                                }
                                else if (propertyType == typeof(float))
                                {
                                    var typedValue = reader.GetSingle();
                                    property.SetValue(twinInstance, typedValue);
                                }
                                else if (propertyType == typeof(decimal))
                                {
                                    var typedValue = reader.GetDecimal();
                                    property.SetValue(twinInstance, typedValue);
                                }
                                else if (propertyType == typeof(byte))
                                {
                                    var typedValue = reader.GetByte();
                                    property.SetValue(twinInstance, typedValue);
                                }
                                else if (propertyType == typeof(sbyte))
                                {
                                    var typedValue = reader.GetSByte();
                                    property.SetValue(twinInstance, typedValue);
                                }
                                else if (propertyType == typeof(ushort))
                                {
                                    var typedValue = reader.GetUInt16();
                                    property.SetValue(twinInstance, typedValue);
                                }
                                else if (propertyType == typeof(uint))
                                {
                                    var typedValue = reader.GetUInt32();
                                    property.SetValue(twinInstance, typedValue);
                                }
                                else if (propertyType == typeof(ulong))
                                {
                                    var typedValue = reader.GetUInt64();
                                    property.SetValue(twinInstance, typedValue);
                                }
                                else if (propertyType.IsClass)
                                {
                                    var twinComponentConverterType = typeof(ObjectToTwinComponentConverter<>);
                                    var concreteTwinComponentConverterType =
                                        twinComponentConverterType.MakeGenericType(propertyType);
                                    var componentConverter = Activator.CreateInstance(concreteTwinComponentConverterType) as JsonConverter;

                                    var componentSerializationOptions = new JsonSerializerOptions()
                                    {
                                        IgnoreNullValues = options.IgnoreNullValues,
                                        Converters = { componentConverter! },
                                        WriteIndented = options.WriteIndented
                                    };
                                    var typedValue =
                                        JsonSerializer.Deserialize(ref reader, propertyType, componentSerializationOptions);
                                    property.SetValue(twinInstance, typedValue);
                                }
                            }
                        }
                    }
                }
            }

            return (T)twinInstance!;
        }

        protected static List<KeyValuePair<string, object?>> GetTwinOnlyPropertyPairs(T twin, string[] twinOnlyPropertyNames)
        {
            var jsonSpecialTwinProperties = GetTwinOnlyProperties(twinOnlyPropertyNames)
                .ToDictionary(prop => prop.GetTwinPropertyName(), prop => prop.GetValue(twin));
            return jsonSpecialTwinProperties.ToList();
        }

        protected static IEnumerable<PropertyInfo> GetTwinOnlyProperties(string[] twinOnlyPropertyNames, string[] twinOnlyPropertyNamesToExclude = null!)
        {
            var properties =
                GetTwinProperties<TwinOnlyPropertyAttribute>(BindingFlags.Public | BindingFlags.NonPublic |
                                                             BindingFlags.Instance)
                    .Where(prop => twinOnlyPropertyNames.Contains(prop.GetTwinPropertyName()));
            
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (twinOnlyPropertyNamesToExclude != null)
            {
                properties = properties.Where(prop =>
                    !twinOnlyPropertyNamesToExclude.Contains(prop.GetTwinPropertyName()));
            }

            return properties;
        }

        protected static IEnumerable<PropertyInfo> GetNormalTwinProperties(string[] twinOnlySpecialPropertyNames, string[] twinOnlyPropertyNamesToExclude = null!)
        {
            var properties = GetTwinProperties<TwinPropertyAttribute>(twinOnlySpecialPropertyNames,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
                prop => prop.GetTwinPropertyName());

            properties = GetTwinProperties<TwinTelemetryAttribute>(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Concat(properties);
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (twinOnlyPropertyNamesToExclude != null)
            {
                properties = properties.Where(prop =>
                    !twinOnlyPropertyNamesToExclude.Contains(prop.GetTwinPropertyName()));
            }

            return properties;
        }

        protected static IEnumerable<PropertyInfo> GetComponentTwinProperties()
        {
            var properties = GetTwinProperties<TwinComponentAttribute>(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            return properties;
        }

        /// <summary>
        /// Gets the properties of the type T that are decorated with the provided attribute TT
        /// </summary>
        /// <typeparam name="TA">The Attribute type for discovery</typeparam>
        /// <typeparam name="TT">The ModelType on which to do the discovery</typeparam>
        protected static IEnumerable<PropertyInfo> GetDecoratedProperties<TA, TT>() where TA : Attribute
        {
            var t = typeof(TT);
            var matchingProperties = t.GetProperties(BindingFlags.NonPublic | BindingFlags.Public)
                .Where(p => Attribute.IsDefined(p, typeof(TA)));
            return matchingProperties;
        }

        /// <summary>
        /// Prepares the properties of the type T that are decorated with the provided attribute type TT
        /// </summary>
        /// <typeparam name="TA">The Attribute type for discovery</typeparam>
        /// <typeparam name="TT">The ModelType on which to do the discovery</typeparam>
        /// <param name="target"></param>
        protected static Dictionary<string, object> PrepareDecoratedProperties<TA, TT>(TT target = default!) where TA : Attribute
        {
            var result = new Dictionary<string, object>();
            var matchingPropertiesList = GetDecoratedProperties<TA, TT>().ToList();

            matchingPropertiesList.ForEach(p =>
            {
                var name = p.GetJsonPropertyName();
                if (target != null)
                {
                    var val = p.GetValue(target);
                    if (val != null)
                    {
                        if (p.GetJsonPropertyName() != "$metadata")
                        {
                            result.Add(name, val);

                            if (Attribute.IsDefined(p, typeof(TwinComponentAttribute)) &&
                                val is TwinBase twinBase)
                            {
                                twinBase.Metadata.IsComponent = true;
                            }
                        }
                    }
                }
            });

            return result;
        }
    }
}
