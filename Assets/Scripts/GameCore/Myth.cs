using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class Myth {
	[JsonProperty]
	public int id { get; private set;}
	[JsonProperty]
	public int type { get; private set;}
	[JsonProperty]
	public string name { get; private set;}
	[JsonProperty]
	public string text { get; private set;}
	[JsonProperty]
	public string action { get; private set;}
	[JsonProperty]
	public int? processCount { get; private set;}
	[JsonProperty]
	public string processActionText { get; private set;}
	[JsonProperty]
	public int? extensionNum { get; private set;}

	public override string ToString ()
	{
		return string.Format ("[Myth: id={0}, type={1}, name={2}]", id, type, name);
	}
}
