using System;

/// <summary>
/// ��UI��������ݣ�ʹ����������޸�UI
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class UIDataBind : Attribute
{
    public string _type;
    public string _path;
    public string[] _extra;
    public UIDataBind(string type, string path, params string[] extra)
    {
        _type = type;
        _path = path;
        _extra = extra;
    }
}
