using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using System;
using System.Reflection;
using Unity.VisualScripting;
using System.Linq;
using System.Runtime.InteropServices;

public class BaseView : UIBase
{
    public string id;
    public GComponent main;
    public string name;

    /// <summary>
    /// ֻ�ڴ�����ʱ��ִ��
    /// </summary>
    public void OnAwake()
    {
        Bind();
    }

    public void Show()
    {
        main.visible = true;
        OnShow();
    }

    public void Hide()
    {
        main.visible = false;
        OnHide();
    }

    public bool GetVisible()
    {
        return main.visible;
    }

    /// <summary>
    /// ÿ��չʾ��ʱ��ִ��
    /// </summary>
    protected virtual void OnShow() { }

    /// <summary>
    /// ÿ�����ص�ʱ��ִ��
    /// </summary>
    protected virtual void OnHide() { }
}
