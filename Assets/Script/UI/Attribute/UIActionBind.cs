using System;

/// <summary>
/// 绑定UI组件和动作事件，使其支持交互
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class UIActionBind : Attribute
{
    public UIAction _type;

    public string _path;

    public string[] _extra;

    public UIActionBind(UIAction type,string path,params string[] extra)
    {
        _type = type;
        _path = path;
        _extra = extra;
    }
}
