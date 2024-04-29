﻿using System.Collections.Generic;
using FairyGUI;

public class UIManager : Singleton<UIManager>
{
    private UINode _root;

    private int _id = 0;

    private List<GComponent> _layer = new List<GComponent>();

    public void Init()
    {
        _root = new UINode();
    }

    /// <summary>
    /// 展示UI
    /// </summary>
    /// <param name="folder"></param>
    /// <param name="package"></param>
    /// <param name="uiName"></param>
    /// <param name="parent"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public UINode ShowUI<T>(string folder, string package, string uiName, UINode parent = null)
        where T : BaseView, new()
    {
        //包加载逻辑暂时加载Resources文件夹内文件 如有需要可自行修改
        string packagePath = folder + "/" + package;
        UIPackage.AddPackage(packagePath);
        //创建UI
        T view = new T();
        view.id = "ui_" + _id++;
        view.main = UIPackage.CreateObject(package, uiName).asCom;

        view.OnAwake();
        view.Show();

        //创建UI节点
        UINode ui = new UINode();
        ui.ui = view;

        if (parent != null)
        {
            int layerIndex = parent.layer + 1;
            if (_layer.Count - 1 == parent.layer)
            {
                GComponent layer = new GComponent();
                layer.displayObject.gameObject.name = "Layer_" + layerIndex;
                GRoot.inst.AddChild(layer);
                _layer.Add(layer);
            }

            _layer[layerIndex].AddChild(view.main);
            
            parent.children.Add(view.id, ui);
            ui.parent = parent;
            ui.layer = layerIndex;
        }
        else
        {
            if (_layer.Count == 0)
            {
                GComponent layer = new GComponent();
                layer.displayObject.gameObject.name = "Layer_0";
                GRoot.inst.AddChild(layer);
                _layer.Add(layer);
            }

            _layer[0].AddChild(view.main);

            _root.children.Add(view.id, ui);
            ui.parent = _root;
            ui.layer = 0;
        }

        view.uiNode = ui;

        return ui;
    }

    /// <summary>
    /// 隐藏UI
    /// </summary>
    /// <param name="ui"></param>
    public void HideUI(UINode ui)
    {
        foreach (var child in ui.children)
        {
            UINode uiChild = child.Value;
            HideUI(uiChild);
        }

        ui.ui.Hide();
    }

    /// <summary>
    /// 根据名字获取UI
    /// </summary>
    /// <param name="name"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    public UINode GetUI(string name, UINode parent = null)
    {
        if (parent == null)
        {
            parent = _root;
        }

        if (name == parent.ui.name)
        {
            return parent;
        }

        foreach (var child in parent.children)
        {
            GetUI(name, child.Value);
        }

        return null;
    }

    /// <summary>
    /// 重新置于上层
    /// </summary>
    /// <param name="ui"></param>
    public void ResetTop(UINode ui)
    {
        _layer[ui.layer].AddChild(ui.ui.main);
    }
}