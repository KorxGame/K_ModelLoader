using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public static class DictionaryHelper 
{



    public static void Set_Value<T>(this Dictionary<string,T> dictionary ,string key , T t)
    {
        if (dictionary.ContainsKey(key))
            dictionary[key] = t;
        else
            dictionary.Add(key, t);
    }
    
}
