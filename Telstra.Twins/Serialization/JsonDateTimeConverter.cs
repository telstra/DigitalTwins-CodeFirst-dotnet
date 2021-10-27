using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Telstra.Twins.Serialization
{
    public class JsonDateTimeConverter : JsonConverter<DateTimeOffset>
    {
        public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var date = reader.GetString();
            DateTimeOffset result;

            // First see if it's in ISO format
            if (DateTimeOffset.TryParse(date, out result))
                return result;

            // could be in American format when calling GetDigitalTwin
            if (DateTimeOffset.TryParseExact(date, "MM/dd/yyyy HH:mm:ss", CultureInfo.CurrentCulture, DateTimeStyles.AssumeUniversal, out result))
                return result;

            throw new FormatException($"DateTimeOffset not in correct format: {date}");
        }

        public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options) 
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
