﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Newtonsoft.Json.Converters{
	public class ObjectOrEmptyArrayConverter<T> : JsonConverter
	{
		public override bool CanWrite { get { return false; } }
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(T);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			switch (reader.TokenType)
			{
			case JsonToken.StartArray:
				reader.Read();
				if (reader.TokenType == JsonToken.EndArray)
					return null;
				else
					throw new JsonSerializationException("Non-empty JSON array does not make a valid Dictionary!");
			case JsonToken.Null:
				return null;
			case JsonToken.StartObject:
				var tw = new System.IO.StringWriter();
				var writer = new JsonTextWriter(tw);
				writer.WriteStartObject();
				int initialDepth = reader.Depth;
				while (reader.Read() && reader.Depth > initialDepth)
				{
					writer.WriteToken(reader);
				}
				writer.WriteEndObject();
				writer.Flush();
				return JsonConvert.DeserializeObject<T>(tw.ToString());
			default:
				throw new JsonSerializationException("Unexpected token!");
			}
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}
	}
}