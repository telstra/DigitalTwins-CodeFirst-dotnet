using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Telstra.Twins.Serialization
{
    public class JsonIntegerConverter : JsonConverter
    {

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(IDictionary<string, object>);
        }

        public override bool CanWrite => false;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var result = new Dictionary<string, object>();
            reader.Read();

            while (reader.TokenType == JsonToken.PropertyName)
            {
                var propertyName = (string)reader.Value;
                reader.Read();
                object value;
                if (reader.TokenType == JsonToken.Integer)
                {
                    var temp = Convert.ToInt64(reader.Value);
                    if (temp <= Byte.MaxValue && temp >= Byte.MinValue)
                        value = Convert.ToByte(reader.Value);
                    else if (temp >= Int16.MinValue && temp <= Int16.MaxValue)
                        value = Convert.ToInt16(reader.Value);
                    else if (temp >= Int32.MinValue && temp <= Int32.MaxValue)
                        value = Convert.ToInt32(reader.Value);
                    else
                        value = temp;
                }
                else
                    value = serializer.Deserialize(reader);
                result.Add(propertyName, value);
                reader.Read();
            }

            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) => throw new NotImplementedException();
    }
}
