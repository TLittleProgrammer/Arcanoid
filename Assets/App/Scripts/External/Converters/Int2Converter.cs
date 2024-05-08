using System;
using Newtonsoft.Json;
using Unity.Mathematics;

namespace App.Scripts.External.Converters
{
    public class Int2Converter : JsonConverter<int2>
    {
        public override bool CanRead => true;
        
        public override void WriteJson(JsonWriter writer, int2 value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            
            writer.WritePropertyName("x", true);
            writer.WriteValue(value.x);
            
            writer.WritePropertyName("y", true);
            writer.WriteValue(value.y);
            
            writer.WriteEndObject();
        }

        public override int2 ReadJson(JsonReader reader, Type objectType, int2 existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var result = default(int2);
            var propertyName = string.Empty;

            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    propertyName = (string)reader.Value;
                }
                else if (reader.TokenType == JsonToken.Integer)
                {
                    if (propertyName == "x")
                    {
                        result.x = (int) (long) reader.Value;
                    }
                    
                    if (propertyName == "y")
                    {
                        result.y = (int) (long) reader.Value;
                    }
                }
                else
                {
                    break;
                }
            }

            return result;
        }
    }
}