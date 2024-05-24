using System;

namespace ReflectionUI
{
    /// <summary>
    /// 绑定 UI 类功能
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class UIClassBind : Attribute
    {
        public UIClass type;

        public string[] extra;

        public UIClassBind(UIClass type, params string[] extra)
        {
            this.type = type;
            this.extra = extra;
        }
    }
}
