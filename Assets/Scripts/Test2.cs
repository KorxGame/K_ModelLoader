using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test2 : MonoBehaviour
{
    public string name = "chahu(size=0.2)";


    private void Start()
    {

        if (GetSizeByFileName(name , out float size))
        {
            print(size);
        }
    }

    bool GetSizeByFileName(string fileName , out float size)
    {
        string[] strs = fileName.SplitFromTo('(', ')');
        
        foreach (var str in strs)
        {
            if (str.Contains("size="))
            {
                if (float.TryParse(str.Replace("size=", String.Empty), out size))
                {
                    return true;
                }
            }
        }
        size = 1;
        return false;
    }
   
}