using UnityEngine;
using System;
using System.Text;
using System.IO;
using System.Collections.Generic;

namespace Newtonsoft.Json.Converters
{
    /// <summary>
    /// Safely checks the value of each property to ensure that we can cast as expected...
    /// </summary>
    public class SafeDictionaryConverter<T, O> : JsonConverter where T : Dictionary<string, O>
    {
        #region Properties
        public override bool CanWrite
        {
            get
            {
                return false;
            }
        }
        #endregion

        #region Implemented Methods
        public override object ReadJson(JsonReader reader, System.Type objectType, object existingValue, JsonSerializer serializer)
        {
            Dictionary<string, O> dictionary = new Dictionary<string, O>();

            int initialDepth = reader.Depth;

            // Read through each property and parse the value...
            while (reader.Read() && reader.Depth > initialDepth)
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    KeyValuePair<string, O> property = ReadProperty(reader);

                    dictionary.Add(property.Key, property.Value);
                }
            }

            return dictionary;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }
        #endregion

        #region Private Deserialization Methods
        /// <summary>
        /// Safely parses the property ensuring that the expected JSON matches the Type.
        /// </summary>
        /// <returns>The property.</returns>
        /// <param name="reader">Reader.</param>
        private KeyValuePair<string, O> ReadProperty(JsonReader reader)
        {
            string key = (string)reader.Value;
            O innerValue = default(O);

            reader.Read();

            if (reader.TokenType == JsonToken.StartArray)
            {
                // We were given an array and expect an object...
                if (!typeof(O).IsArray)
                {
                    innerValue = Activator.CreateInstance<O>();
                }
                else
                {
                    // All is good, let's continue...
                    innerValue = ReadPropertyValue(reader);
                }
            }

            if (reader.TokenType == JsonToken.StartObject)
            {
                // TODO: Perhaps check to see if we expect an array here...
                innerValue = ReadPropertyValue(reader);
            }

            return new KeyValuePair<string, O>(key, (O)innerValue);
        }

        /// <summary>
        /// Captures the inner json and deserializes it into the appropriate value type..
        /// </summary>
        /// <returns>The property value.</returns>
        /// <param name="reader">Reader.</param>
        private O ReadPropertyValue(JsonReader reader)
        {
            int initialDepth = reader.Depth;

            using (StringWriter stringWriter = new StringWriter())
            {
                JsonTextWriter jsonWriter = new JsonTextWriter(stringWriter);
                jsonWriter.WriteStartObject();

                while (reader.Read() && reader.Depth > initialDepth)
                {
                    jsonWriter.WriteToken(reader);
                }

                jsonWriter.WriteEndObject();
                jsonWriter.Flush();

                return JsonConvert.DeserializeObject<O>(stringWriter.ToString());
            }
        }
        #endregion
    }
}