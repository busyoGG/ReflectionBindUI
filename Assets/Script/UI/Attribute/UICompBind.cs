using System;

/// <summary>
/// ��UI���
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class UICompBind :Attribute
{
    public UIType _type;
    public string _path;
    public UICompBind(UIType type,string path)
    {
        _type = type;
        _path = path;
    }
}
