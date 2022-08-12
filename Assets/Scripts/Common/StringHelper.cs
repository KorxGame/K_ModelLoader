using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StringHelper
{
    
    
    /// <summary>
    /// string根据字符拆解
    /// "(size=2)))(()(1233(aaaa)" ==> size=2 aaaa
    /// </summary>
    /// <param name="str"></param>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns></returns>
    public static string[] SplitFromTo(this string str ,char from, char to) //
    {
        // string[] strs = name.Split(new[] { from, to });
        // return strs;
        string[] strs = str.Split(from);
        List<string> list_strs = new List<string>();

        foreach (var value in strs)
        {
            if (value.Contains(to.ToString()) )
            {
                string content = value.Split(to)[0];
                if (!string.IsNullOrEmpty(content))
                {
                    list_strs.Add(content);
                }
            }
        }
        return list_strs.ToArray();
    }
    
    
    
    
    
}
