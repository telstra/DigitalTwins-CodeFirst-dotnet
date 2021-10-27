using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Telstra.Twins.Core
{
    public interface IDigitalTwinSerializer
    {
        object DeserializeTwin(string json, JsonSerializerOptions serializerOptions = null);
        T DeserializeTwin<T>(string json, JsonSerializerOptions serializerOptions = null);
        string SerializeModel(Type twinType, bool htmlEncode = false, JsonSerializerOptions serializerOptions = null);
        string SerializeModel<T>(bool htmlEncode = false, JsonSerializerOptions serializerOptions = null);
        string SerializeTwin(object value, bool htmlEncode = false, JsonSerializerOptions serializerOptions = null);
        string SerializeTwin<T>(T value, bool htmlEncode = false, JsonSerializerOptions serializerOptions = null);
    }
}
