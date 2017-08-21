
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using UnityEngine;

public static class Extensions {
    public static string Print (this object obj, int depth = 0, string name = ""){
        string result = string.Empty;
        string tabs = new string ('\t', depth);

        if(obj == null){
            return  " Null";
        }
        var props = obj.GetType ().GetProperties ().Where(prop => Attribute.IsDefined(prop, typeof(JsonPropertyAttribute))).ToList();

        if (props.Count > 0) {
            foreach(var prop in props){
                PropertyInfo pi =  obj.GetType().GetProperty(prop.Name, BindingFlags.Instance);
                object val = pi.GetValue(obj, null);
                if(obj is IDictionary){
                    result += "\n" + tabs + "["+prop.Name+"] {\n" + (val as IDictionary).ToString(" = ", "\n", depth + 1) + tabs + "}";
                } else {
                    result += "\n" + tabs + "["+prop.Name+"] " + val.Print (depth + 1, prop.Name);
                }
            }
        } else {
            if(obj is IList){
                result += "\t{\n";
                foreach(var item in (obj as IList)){
                    result += tabs + "\t"  + item.Print(depth + 2) + "\n";
                }
                result += tabs + "}";
            } else if (obj is IDictionary){
                IDictionary dict = (obj as IDictionary);
                result += tabs  + dict.ToString(" = ", "\n", depth + 1);
            } else {
                result += obj.ToString ();
            }
        }

        return result;
    }

    public static void PrintToFile (this object obj, string path){
        string result = obj.Print();

        File.WriteAllText (path, result);
        Debug.Log ("Log writed to " + path);
    }
    
    public static string ToString(this IDictionary source, string keyValueSeparator, string sequenceSeparator, int depth = 0) 
    { 
        string result = "\n";
        if (source == null) 
            return "Null";
        if (source.Count == 0) {
            return "Empty";
        }
        string tabs = new string ('\t', depth);

        foreach (var item in source) {
            Type itemType = item.GetType ();
            if (itemType.IsAssignableFrom(typeof(DictionaryEntry))) {

                DictionaryEntry entry = (DictionaryEntry) item;
                result += tabs + "[" +  entry.Key + "]" + keyValueSeparator + entry.Value.Print (depth) + sequenceSeparator;
            }
        }

        return result;
    }
}