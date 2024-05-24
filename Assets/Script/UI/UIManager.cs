using System;
using System.Collections.Generic;
using FairyGUI;

namespace ReflectionUI
{
    public class UIManager : Singleton<UIManager>
    {
        private UINode _root = new UINode();

        private int _id = 0;

        private List<GComponent> _layer = new List<GComponent>();

        private Dictionary<string, UINode> _savedView = new Dictionary<string, UINode>();

        /// <summary>
        /// 展示UI
        /// </summary>
        /// <param name="folder">UI所在文件夹</param>
        /// <param name="package">UI包名</param>
        /// <param name="name">自定义名称</param>
        /// <param name="parent">父节点</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public UINode ShowUI<T>(string folder, string package, string name, UINode parent = null)
            where T : BaseView, new()
        {
            //有保存的UI直接展示并返回
            if (_savedView.TryGetValue(name, out var node))
            {
                node.ui.Show();
                return node;
            }

            //创建没保存的UI
            Type type = typeof(T);
            //包加载逻辑暂时加载Resources文件夹内文件 如有需要可自行修改
            string packagePath = folder + "/" + package;
            UIPackage.AddPackage(packagePath);
            //创建UI
            T view = new T();
            view.id = "ui_" + _id++;
            view.name = name;
            view.main = UIPackage.CreateObject(package, type.Name).asCom;

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

            view.OnAwake();
            view.Show();

            return ui;
        }

        /// <summary>
        /// 隐藏UI
        /// </summary>
        /// <param name="ui">UI节点</param>
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
        /// <param name="name">自定义名称</param>
        /// <param name="parent">父节点</param>
        /// <returns></returns>
        public UINode GetUI(string name, UINode parent = null)
        {
            if (parent == null)
            {
                parent = _root;
            }

            if (parent.ui != null && name == parent.ui.name)
            {
                return parent;
            }

            UINode node;

            foreach (var child in parent.children)
            {
                node = GetUI(name, child.Value);
                if (node != null)
                {
                    return node;
                }
            }

            return null;
        }

        /// <summary>
        /// 销毁UI
        /// </summary>
        /// <param name="ui">UI节点</param>
        public void DisposeUI(UINode ui)
        {
            foreach (var child in ui.children)
            {
                UINode uiChild = child.Value;
                DisposeUI(uiChild);
            }

            //移除保存的节点
            _savedView.Remove(ui.ui.name);
            ui.children = null;
            ui.parent = null;
            ui.ui.Dispose();
        }

        /// <summary>
        /// 重新置于上层
        /// </summary>
        /// <param name="ui">UI节点</param>
        public void ResetTop(UINode ui)
        {
            _layer[ui.layer].AddChild(ui.ui.main);
            ConsoleUtils.Log("重新置顶");
        }

        /// <summary>
        /// 设置模态背景
        /// </summary>
        /// <param name="ui">UI节点</param>
        /// <param name="model">模态背景对象</param>
        public void SetModel(UINode ui, GGraph model)
        {
            GComponent layer = _layer[ui.layer];
            int index = layer.GetChildIndex(ui.ui.main);
            layer.AddChildAt(model, index);
        }

        /// <summary>
        /// 保存节点 需要ui名称唯一
        /// </summary>
        /// <param name="name">自定义名称</param>
        /// <param name="ui">UI节点</param>
        public void SaveNode(string name, UINode ui)
        {
            _savedView[name] = ui;
        }
    }
}