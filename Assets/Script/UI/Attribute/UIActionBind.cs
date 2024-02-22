using System;

/// <summary>
/// ��UI����Ͷ����¼���ʹ��֧�ֽ���
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class UIActionBind : Attribute
{
    public string _type;

    public string _path;

    public UIActionBind(string type,string path)
    {
        _type = type;
        _path = path;
    }
}
