using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;


namespace Newtonsoft.Json.Converters
{
    public class DictionaryContainingDictionaryValueConverter<Tkey, TValkey, TValvalue> : JsonConverter
    {
        public override bool CanWrite { get { return false; } }
        public override bool CanConvert (Type objectType) {
            return typeof(Dictionary<Tkey, Dictionary<TValkey, TValvalue>>).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            switch (reader.TokenType) {
                case JsonToken.StartArray:
                    reader.Read();
                    if (reader.TokenType == JsonToken.EndArray)
                        return null;
                    else
                        throw new JsonSerializationException("Non-empty JSON array does not make a valid Dictionary!");
                case JsonToken.Null:
                    return null;
                case JsonToken.StartObject:
                    int initialDepth = reader.Depth;
                    Dictionary<Tkey, Dictionary<TValkey, TValvalue>> resultDict = new Dictionary<Tkey, Dictionary<TValkey, TValvalue>> ();
                    while (reader.Read () && reader.Depth > initialDepth) {
                        if (reader.Depth == initialDepth + 1) {

                            Tkey currKey = JsonConvert.DeserializeObject<Tkey> (reader.Value.ToString ());
                            reader.Read();
                            reader.Read();
                            if (reader.Depth == initialDepth + 3) {
                                Dictionary<TValkey, TValvalue> currValue = new Dictionary<TValkey, TValvalue> ();
                                while (reader.Depth == initialDepth + 3) {
                                    TValkey key = JsonConvert.DeserializeObject<TValkey>(reader.Value.ToString());
                                    reader.Read ();
                                    TValvalue value = JsonConvert.DeserializeObject<TValvalue> (reader.Value.ToString ());
                                    currValue[key] = value;
                                    reader.Read();
                                }
                                resultDict[currKey] = currValue;
                            } else {
                                resultDict[currKey] = new Dictionary<TValkey, TValvalue>();
                            }
                        }
                    }
                    return resultDict;
                default:
                    throw new JsonSerializationException("Unexpected token!");
            }
        }

        public override void WriteJson (JsonWriter writer, object value, JsonSerializer serializer) {
            throw new NotImplementedException ();
        }
    }
}
