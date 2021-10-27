using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Telstra.Twins.Attributes;
using Telstra.Twins.Common;
using Telstra.Twins.Enums;
using Telstra.Twins.Helpers;
using Telstra.Twins.Models;
using Telstra.Twins.Serialization;
using Telstra.Twins.Services;

namespace Telstra.Twins.Core
{
    /// <summary>
    /// Serializes and deserializes classes and objects into and from the JSON DTDL v2 format.
    /// The <see cref="DigitalTwinSerializer"/> enables you to control how classes and objects are encoded into DTDL.
    /// </summary>
    public class DigitalTwinSerializer : IDigitalTwinSerializer
    {
        public IModelLibrary ModelLibrary { get; }

        public DigitalTwinSerializer(IModelLibrary modelLibrary)
        {
            this.ModelLibrary = modelLibrary;
        }

        /// <summary>
        /// Deserializes the provided JSON to the type indicated by the provided JSON.
        /// </summary>
        /// <param name="json">The JSON string to deserialize.</param>
        /// <param name="serializerOptions">Optional serialization options.</param>
        /// <returns>An instance of <see cref="object"/> deserialized from JSON.</returns>
        /// <exception cref="System.ArgumentException">Thrown when the type cannot be found
        /// or if the type is unknown in the provided JSON</exception>
        /// <exception cref="System.Text.Json.JsonException">The JSON is invalid.</exception>
        public object DeserializeTwin(string json, JsonSerializerOptions serializerOptions = null)
        {
            var modelType = this.ModelLibrary.GetById(JsonHelpers.GetModelId(json)) ?? typeof(GenericTwin);
            var options = serializerOptions ?? GetTwinSerializationSettings();
            var twinConverterType = typeof(ObjectToTwinConverter<>);
            var concreteTwinConverterType = twinConverterType.MakeGenericType(modelType);
            var converter = Activator.CreateInstance(concreteTwinConverterType);

            options.Converters.Add(converter as JsonConverter);
            var result = JsonSerializer.Deserialize(json, modelType, options);
            return result;
        }

        /// <summary>
        /// Deserializes the provided JSON to the type indicated by the type parameter.
        /// </summary>
        /// <param name="type">The type of the destination twin.</param>
        /// <param name="json">The JSON string to deserialize.</param>
        /// <param name="serializerOptions">Optional serialization options.</param>
        /// <returns>An instance of <see cref="object"/> deserialized from JSON.</returns>
        /// <exception cref="System.ArgumentException">Thrown when the type cannot be found
        /// or if the type is unknown in the provided JSON</exception>
        /// <exception cref="System.Text.Json.JsonException">The JSON is invalid.</exception>
        public object DeserializeTwin(Type type, string json, JsonSerializerOptions serializerOptions = null)
        {
            var options = serializerOptions ?? GetTwinSerializationSettings();
            var twinConverterType = typeof(ObjectToTwinConverter<>);
            var concreteTwinConverterType = twinConverterType.MakeGenericType(type);
            var converter = Activator.CreateInstance(concreteTwinConverterType);

            options.Converters.Add(converter as JsonConverter);
            var result = JsonSerializer.Deserialize(json, type, options);
            return result;
        }

        /// <summary>
        /// Deserializes the JSON to the specified twin type.
        /// </summary>
        /// <typeparam name="T">Deserialization target type</typeparam>
        /// <param name="json">JSON to deserialize</param>
        /// <param name="serializerOptions">Optional serialization options.</param>
        /// <returns>An instance of <typeparamref name="T"/> deserialized from JSON</returns>
        public T DeserializeTwin<T>(string json, JsonSerializerOptions serializerOptions = null)
        {
            var options = serializerOptions ?? GetTwinSerializationSettings();
            options.Converters.Add(new ObjectToTwinConverter<T>());
            var result = JsonSerializer.Deserialize<T>(json, options);
            return result;
        }

        /// <summary>
        /// Deserializes the twin's model from JSON to the type defined by T with default serialization settings.
        /// </summary>
        /// <typeparam name="T">The type of the model to be used.</typeparam>
        /// <param name="json">JSON to deserialize</param>
        /// <param name="serializerOptions">Optional serialization options.</param>
        /// <returns>A blank instance of the newly deserialized model type.</returns>
        public T DeserializeModel<T>(string json, JsonSerializerOptions serializerOptions = null)
        {
            var options = serializerOptions ?? GetModelSerializationSettings();
            options.Converters.Add(new ClassToTwinModelConverter<T>());
            var instanceOfModelType = JsonSerializer.Deserialize<T>(json, options);
            return instanceOfModelType;
        }

        /// <summary>
        /// Serializes the twin's model to JSON with default serialization settings.
        /// </summary>
        /// <param name="twinType">The <see cref="System.Type"/> used to generate the digital twin model.</param>
        /// <param name="htmlEncode">If true, ensures the result is HTML encoded. The default is false.</param>
        /// <param name="serializerOptions">Optional serialization options.</param>
        /// <returns>The serialized JSON string</returns>
        public string SerializeModel(Type twinType, bool htmlEncode = false, JsonSerializerOptions serializerOptions = null)
        {
            var options = serializerOptions ?? GetModelSerializationSettings();
            var modelConverterType = typeof(ClassToTwinModelConverter<>);
            var concreteModelConverterType = modelConverterType.MakeGenericType(twinType);
            var converter = Activator.CreateInstance(concreteModelConverterType);
            var instance = Activator.CreateInstance(twinType);
            options.Converters.Add(converter as JsonConverter);
            var result = JsonSerializer.Serialize(instance, options);
            return result;
        }

        /// <summary>
        /// Serializes the twin's model to JSON with default serialization settings.
        /// </summary>
        /// <typeparam name="T">The type of the model to be used.</typeparam>
        /// <param name="htmlEncode">If true, ensures the result is HTML encoded. The default is false.</param>
        /// <param name="serializerOptions">Optional serialization options.</param>
        /// <returns>The serialized JSON string</returns>
        public string SerializeModel<T>(bool htmlEncode = false, JsonSerializerOptions serializerOptions = null)
        {
            var options = serializerOptions ?? GetModelSerializationSettings();
            options.Converters.Add(new ClassToTwinModelConverter<T>());
            var instance = Activator.CreateInstance<T>();
            var result = JsonSerializer.Serialize<T>(instance, options);
            return result;
        }

        /// <summary>
        /// Serializes the twin to JSON.
        /// </summary>
        /// <param name="twin">The twin object to be serialized.</param>
        /// <param name="htmlEncode">If true, ensures the result is HTML encoded. The default is false.</param>
        /// <param name="serializerOptions">Optional serialization options.</param>
        /// <returns>The serialized JSON string</returns>
        public string SerializeTwin(object twin, bool htmlEncode = false, JsonSerializerOptions serializerOptions = null)
        {
            var options = serializerOptions ?? GetTwinSerializationSettings();
            var twinConverterType = typeof(ObjectToTwinConverter<>);
            var concreteTwinConverterType = twinConverterType.MakeGenericType(twin.GetType());
            var converter = Activator.CreateInstance(concreteTwinConverterType);

            options.Converters.Add(converter as JsonConverter);
            var result = JsonSerializer.Serialize(twin, options);
            return result;
        }

        /// <summary>
        /// Serializes the twin to JSON with default serialization settings.
        /// </summary>
        /// <typeparam name="T">The type of the object to be serialized.</typeparam>
        /// <param name="twin">The twin object to be serialized.</param>
        /// <param name="htmlEncode">If true, ensures the result is HTML encoded. The default is false.</param>
        /// <param name="serializerOptions">Optional serialization options.</param>
        /// <returns>The serialized JSON string</returns>
        public string SerializeTwin<T>(T twin, bool htmlEncode = false, JsonSerializerOptions serializerOptions = null)
        {
            var options = serializerOptions ?? GetTwinSerializationSettings();
            options.Converters.Add(new ObjectToTwinConverter<T>());
            var result = JsonSerializer.Serialize<T>(twin, options);
            return result;
        }

        /// <summary>
        /// Twin serializer settings
        /// </summary>
        private static readonly JsonSerializerOptions TwinSerializationSettings = GetTwinSerializationSettings();

        private static JsonSerializerOptions GetTwinSerializationSettings() => new JsonSerializerOptions()
        {
            IgnoreNullValues = true,
            Converters =
            {
                new JsonStringEnumConverter()
            },
            WriteIndented = true
        };

        /// <summary>
        /// Model serializer settings
        /// </summary>
        private static readonly JsonSerializerOptions ModelSerializationSettings =
            GetModelSerializationSettings();

        private static JsonSerializerOptions GetModelSerializationSettings() => new JsonSerializerOptions()
        {
            IgnoreNullValues = true,
            Converters =
            {
                new JsonStringEnumConverter()
            },
            WriteIndented = true
        };
    }
}
