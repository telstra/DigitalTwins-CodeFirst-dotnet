using Newtonsoft.Json;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Telstra.Twins.Serialization
{
    public class NewtonsoftJsonDateTimeConverter : JsonConverter<DateTimeOffset>
    {
        public override DateTimeOffset ReadJson(JsonReader reader, Type objectType, [AllowNull] DateTimeOffset existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var date = reader.ReadAsString();
            DateTimeOffset result;

            // First see if it's in ISO format
            if (DateTimeOffset.TryParse(date, out result))
                return result;

            // could be in American format when calling GetDigitalTwin
            if (DateTimeOffset.TryParseExact(date, "MM/dd/yyyy HH:mm:ss", CultureInfo.CurrentCulture, DateTimeStyles.AssumeUniversal, out result))
                return result;

            return existingValue;
        }


        public override void WriteJson(JsonWriter writer, [AllowNull] DateTimeOffset value, JsonSerializer serializer) =>
            writer.WriteValue(value.ToString());
    }
}
