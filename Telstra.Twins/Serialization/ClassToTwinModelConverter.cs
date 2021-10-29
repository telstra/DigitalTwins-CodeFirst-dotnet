using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Telstra.Twins.Attributes;
using Telstra.Twins.Helpers;
using Telstra.Twins.Models;

namespace Telstra.Twins.Serialization
{
    public class ClassToTwinModelConverter<T> : TwinConverterBase<T>
    {
        public string[] SpecialModelPropertyNames { get; } = { "@id", "@type", "extends", "@context", "displayName" };

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            //
            // First, write the reserved model properties
            //
            var specialModelProperties =
                GetModelOnlyProperties(value, this.SpecialModelPropertyNames);

            // If the twin model Id hasn't been set in the POCO, then generate it from the type information
            if (specialModelProperties.TrueForAll(pair => pair.Key != "@id"))
            {
                specialModelProperties.Insert(0, new KeyValuePair<string, object>("@id", typeof(T).GetDigitalTwinModelId()));
            }

            if (!specialModelProperties.Any(pair => pair.Key == "extends" && pair.Value != null))
            {
                specialModelProperties.RemoveAll(pair => pair.Key == "extends");
                var insertIndex = specialModelProperties.Count > 2 ? 2 : 0;
                specialModelProperties.Insert(insertIndex, new KeyValuePair<string, object>("extends", typeof(T).GetDigitalTwinModelExtends()));
            }

            // TODO: Add handling of missing extends, displayName, and @type here

            foreach (var pair in specialModelProperties.Where(p => p.Value != null))
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
            // Next, read the contents of the model
            //
            var contents = new List<Content>();
            AddPropertiesContent(contents);
            AddModelRelationshipsContent(contents);
            AddComponentsContent(contents);
            AddTelemetryContent(contents);

            //
            // Finally, write the contents out in an array of content types
            //
            if (contents.Any())
            {
                writer.WriteStartArray("contents");
                contents.ForEach(contentItem =>
                {
                    JsonSerializer.Serialize<object>(writer, contentItem, options);
                });

                writer.WriteEndArray();
            }

            writer.WriteEndObject();
        }

        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var specialModelProperties = GetModelOnlyProperty(this.SpecialModelPropertyNames);

            var propMap = specialModelProperties
                .ToDictionary(
                    prop => prop.GetModelPropertyName(),
                    prop => prop);
            var twinInstance = Activator.CreateInstance(typeToConvert);

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
                                property.SetValue(twinInstance, typedValue);
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
                        }
                    }
                }
            }

            return (T)twinInstance;
        }

        protected static void AddTelemetryContent(List<Content> contents)
        {
            var typeToAnalyze = typeof(T);
            var modelProperties = typeToAnalyze.GetModelTelemetry()
                .ToList()
                .Select(ModelProperty.Create);
            contents.AddRange(modelProperties);
        }

        protected static void AddComponentsContent(List<Content> contents)
        {
            var typeToAnalyze = typeof(T);
            var modelProperties = typeToAnalyze.GetModelComponents()
                .ToList()
                .Select(ModelComponent.Create);
            contents.AddRange(modelProperties);
        }

        protected static void AddPropertiesContent(List<Content> contents)
        {
            var typeToAnalyze = typeof(T);
            var modelProperties = typeToAnalyze.GetModelProperties()
                .ToList()
                .Select(ModelProperty.Create);
            contents.AddRange(modelProperties);
        }

        protected static void AddModelRelationshipsContent(List<Content> contents)
        {
            var typeToAnalyze = typeof(T);
            var modelProperties = typeToAnalyze.GetModelRelationships()
                .ToList()
                .Select(ModelRelationship.Create);
            contents.AddRange(modelProperties);
        }

        protected static List<KeyValuePair<string, object>> GetModelOnlyProperties(T twin, string[] modelOnlyPropertyNames)
        {
            var jsonSpecialModelProperties = GetModelOnlyProperty(modelOnlyPropertyNames)
                .ToDictionary(prop => prop.GetModelPropertyName(), prop => prop.GetValue(twin));
            return jsonSpecialModelProperties.ToList();
        }

        protected static IEnumerable<PropertyInfo> GetModelOnlyProperty(string[] modelOnlyPropertyNames)
        {
            var typeToAnalyze = typeof(T);
            var properties = typeToAnalyze
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(prop => prop.IsDefined(typeof(TwinModelOnlyPropertyAttribute)))
                .Where(prop => modelOnlyPropertyNames.Contains(prop.GetModelPropertyName()));
            return properties;
        }
    }
}
