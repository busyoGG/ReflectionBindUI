using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AttributeUsage(AttributeTargets.Class)]
public class UIClassBind : Attribute
{
    public UIClass type;
    
    public string[] extra;
    
    public UIClassBind(UIClass type,params string[] extra)
    {
        this.type = type;
        this.extra = extra;
    }
}
