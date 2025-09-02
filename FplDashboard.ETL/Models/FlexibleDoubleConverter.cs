using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FplDashboard.ETL.Models
{
    public class FlexibleDoubleConverter : JsonConverter<double>
    {
        public override double Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.Number:
                    return reader.GetDouble();
                case JsonTokenType.String:
                {
                    var str = reader.GetString();
                    return double.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out var value) ? value : throw new JsonException($"Cannot convert '{str}' to double.");
                }
                default:
                    throw new JsonException($"Unexpected token parsing double. Token: {reader.TokenType}");
            }
        }

        public override void Write(Utf8JsonWriter writer, double value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value);
        }
    }

    public class FlexibleNullableDoubleConverter : JsonConverter<double?>
    {
        public override double? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
                return null;
            if (reader.TokenType == JsonTokenType.Number)
                return reader.GetDouble();
            if (reader.TokenType == JsonTokenType.String)
            {
                var str = reader.GetString();
                if (string.IsNullOrWhiteSpace(str))
                    return null;
                if (double.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out var value))
                    return value;
                throw new JsonException($"Cannot convert '{str}' to double.");
            }
            throw new JsonException($"Unexpected token parsing double?. Token: {reader.TokenType}");
        }

        public override void Write(Utf8JsonWriter writer, double? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
                writer.WriteNumberValue(value.Value);
            else
                writer.WriteNullValue();
        }
    }
}
