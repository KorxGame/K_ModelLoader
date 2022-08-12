using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorHelper 
{
    public static Color FloatToRGB(this float value)
    {
       return Color.HSVToRGB(1 - value, 1, 1);
    }
}
