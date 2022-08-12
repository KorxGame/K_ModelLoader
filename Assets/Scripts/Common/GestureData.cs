using System;
using System.Collections.Generic;
using UnityEngine;

public class GestureData
{
	public string msg_body;
	public string msg_type;
}

public class Gesture
{
	public byte index;
	public Pos pos;
	public byte state;
	public byte type;
}

public class Pos
{
    public List<float> T;
    public List<float> R;
}
//public class Pos
//{
//	public Vector4 T { get; set; }
//	public Vector4 R { get; set; }
//}