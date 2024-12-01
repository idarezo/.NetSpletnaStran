using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace ozra_vaje4.Models
{
    public class BsonObjectIdConverter : JsonConverter<BsonObjectId>
    {
        public override BsonObjectId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
           
            return BsonObjectId.Parse(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, BsonObjectId value, JsonSerializerOptions options)
        {
            
            writer.WriteStringValue(value.ToString());
        }
    }
}
