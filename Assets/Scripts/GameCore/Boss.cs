using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class Boss {
	[JsonProperty]
	public int id { get; private set;}
	[JsonProperty]
	public string name { get; private set;}
	[JsonProperty]
	private string myth_1 { 
		get{ 
			return JsonConvert.SerializeObject(myths [0]);
		} 
		set{ 
			myths [0] = JsonConvert.DeserializeObject<List<int>>(value);	
		}
	}
	[JsonProperty]
	private string myth_2 { 
		get{ 
			return JsonConvert.SerializeObject(myths [1]);
		} 
		set{ 
			myths [1] = JsonConvert.DeserializeObject<List<int>>(value);	
		}
	}
	[JsonProperty]
	private string myth_3 { 
		get{ 
			return JsonConvert.SerializeObject(myths [2]);
		} 
		set{ 
			myths [2] = JsonConvert.DeserializeObject<List<int>>(value);	
		}
	}

	public Dictionary<int, List<int>> myths = new Dictionary<int, List<int>>(3);

	public override string ToString ()
	{
		string result =  string.Format ("[Boss: id={0}, name={1}", id, name);
		foreach (var t in myths) {
			result += t.Key + "[";
			foreach (int a in t.Value) {
				result += a + ",";		
			}
			result += "]";
		}

		return result;
	}

}
