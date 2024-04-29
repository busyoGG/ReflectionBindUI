using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.All)]
public class UIColor:Attribute
{
    public Color color;

    public UIColor(float r, float g, float b, float a)
    {
        color = new Color(r, g, b, a);
    }
}