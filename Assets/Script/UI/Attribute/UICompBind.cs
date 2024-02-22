using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��UI���
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class UICompBind :Attribute
{
    public string _type;
    public string _path;
    public UICompBind(string type,string path)
    {
        _type = type;
        _path = path;
    }
}
