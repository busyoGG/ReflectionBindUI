using System;

/// <summary>
/// ��UI����Ͷ����¼���ʹ��֧�ֽ���
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class UIActionBind : Attribute
{
    public string _type;

    public string _path;

    public string[] _extra;

    public UIActionBind(string type,string path,params string[] extra)
    {
        _type = type;
        _path = path;
        _extra = extra;
    }
}
