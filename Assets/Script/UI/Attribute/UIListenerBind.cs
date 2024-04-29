using System;

/// <summary>
/// °ó¶¨UI¼àÌıÊÂ¼ş
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class UIListenerBind : Attribute
{
    public string _name;

    public UIListenerBind(string name)
    {
        _name = name;
    }
}
