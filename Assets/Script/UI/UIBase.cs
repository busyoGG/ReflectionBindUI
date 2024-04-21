using FairyGUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class UIBase
{
    public GComponent main;
    public string id;
    public string name;

    private readonly BindingFlags _flag = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static |
                                          BindingFlags.Instance;

    //----- 内建私有变量 -----

    private readonly Dictionary<string, bool> _dropDic = new Dictionary<string, bool>();

    private GameObject _copy;

    private UIDrag _uiDrag;

    private readonly ArrayList _dropData = new ArrayList();

    protected void Bind()
    {
        Type type = GetType();
        PropertyInfo[] props = type.GetProperties(_flag);
        // GComponent main = (GComponent)type.GetField("main", flag).GetValue(this);

        MethodInfo[] methods = type.GetMethods(_flag);

        foreach (var method in methods)
        {
            var methodAttrs = method.GetCustomAttributes(true);
            foreach (var attr in methodAttrs)
            {
                if (attr is UIActionBind)
                {
                    BindAction(method, attr);
                }
                else if (attr is UIListenerBind)
                {
                    BindListener(method, attr);
                }
            }
        }

        foreach (var prop in props)
        {
            var propAttrs = prop.GetCustomAttributes(true);
            foreach (var attr in propAttrs)
            {
                if (attr is UICompBind)
                {
                    BindComp(prop, attr);
                }
                else if (attr is UIDataBind)
                {
                    BindData(prop, attr);
                }
            }
        }
    }

    private void BindComp(PropertyInfo prop, object attr)
    {
        UICompBind uiBind = (UICompBind)attr;

        switch (uiBind._type)
        {
            case UIType.Comp:
                GComponent comp = FguiUtils.GetUI<GComponent>(main, uiBind._path);
                prop.SetValue(this, comp);
                break;
            case UIType.TextField:
                GTextField textField = FguiUtils.GetUI<GTextField>(main, uiBind._path);
                prop.SetValue(this, textField);
                break;
            case UIType.TextInput:
                GTextInput textInput = FguiUtils.GetUI<GTextInput>(main, uiBind._path);
                prop.SetValue(this, textInput);
                break;
            case UIType.Image:
                GImage image = FguiUtils.GetUI<GImage>(main, uiBind._path);
                prop.SetValue(this, image);
                break;
            case UIType.Loader:
                GLoader loader = FguiUtils.GetUI<GLoader>(main, uiBind._path);
                prop.SetValue(this, loader);
                break;
            case UIType.List:
                GList list = FguiUtils.GetUI<GList>(main, uiBind._path);
                prop.SetValue(this, list);
                break;
        }
    }

    private void BindData(PropertyInfo prop, object attr)
    {
        UIDataBind uiBind = (UIDataBind)attr;

        var onValueChange = prop.PropertyType.GetField("_onValueChange", _flag);
        var onUIChange = prop.PropertyType.GetField("_onUIChange", _flag);

        var value = prop.GetValue(this);
        if (value == null)
        {
            Type propType = prop.PropertyType;
            if (propType == typeof(StringUIProp))
            {
                value = new StringUIProp();
            }
            else if (propType == typeof(DoubleUIProp))
            {
                value = new DoubleUIProp();
            }
            else
            {
                Type genericType = typeof(UIListProp<>).MakeGenericType(prop.PropertyType.GenericTypeArguments);
                value = Activator.CreateInstance(genericType);
            }

            prop.SetValue(this, value);
        }

        switch (uiBind._type)
        {
            case UIType.TextField:
                GTextField textField = FguiUtils.GetUI<GTextField>(main, uiBind._path);

                void ActionText(string data)
                {
                    if (textField != null)
                    {
                        textField.text = data;
                    }
                }

                string ActionTextUI()
                {
                    return textField.text;
                }

                onValueChange?.SetValue(value, (Action<string>)ActionText);
                onUIChange?.SetValue(value, (Func<string>)ActionTextUI);
                break;
            case UIType.TextInput:
                GTextInput textInput = FguiUtils.GetUI<GTextInput>(main, uiBind._path);

                void ActionInput(string data)
                {
                    if (textInput != null)
                    {
                        textInput.text = data;
                    }
                }

                string ActionInputUI()
                {
                    return textInput.text;
                }

                onValueChange?.SetValue(value, (Action<string>)ActionInput);
                onUIChange?.SetValue(value, (Func<string>)ActionInputUI);
                break;
            case UIType.Image:
                GImage image = FguiUtils.GetUI<GImage>(main, uiBind._path);

                void ActionImage(string data)
                {
                    if (image != null)
                    {
                        image.icon = data;
                    }
                }

                string ActionImageUI()
                {
                    return image.icon;
                }

                onValueChange?.SetValue(value, (Action<string>)ActionImage);
                onUIChange?.SetValue(value, (Func<string>)ActionImageUI);
                break;
            case UIType.Loader:
                GLoader loader = FguiUtils.GetUI<GLoader>(main, uiBind._path);

                void ActionLoader(string data)
                {
                    if (loader != null)
                    {
                        loader.url = data;
                    }

                    // ConsoleUtils.Log("替换图片", loader?.url);
                }

                string ActionLoaderUI()
                {
                    return loader.url;
                }

                onValueChange?.SetValue(value, (Action<string>)ActionLoader);
                onUIChange?.SetValue(value, (Func<string>)ActionLoaderUI);
                break;
            case UIType.List:
                GList list = FguiUtils.GetUI<GList>(main, uiBind._path);

                void ActionList(int data)
                {
                    if (list != null)
                    {
                        list.SetVirtual();
                        list.numItems = data;

                        if (uiBind._extra.Length > 0)
                        {
                            switch (uiBind._extra[0])
                            {
                                case "height":
                                    if (list.numChildren > 0)
                                    {
                                        list.height = data * list.GetChildAt(0).height + list.lineGap * (data - 1) +
                                                      list.margin.top + list.margin.bottom;
                                    }

                                    break;
                                case "width":
                                    if (list.numChildren > 0)
                                    {
                                        list.width = data * list.GetChildAt(0).width + list.columnGap * (data - 1) +
                                                     list.margin.left + list.margin.right;
                                    }

                                    break;
                            }
                        }
                    }
                }

                onValueChange?.SetValue(value, (Action<int>)ActionList);
                break;
            case UIType.Slider:
                GSlider slider = FguiUtils.GetUI<GSlider>(main, uiBind._path);

                void ActionSlider(double data)
                {
                    if (slider != null)
                    {
                        slider.value = data;
                    }
                }

                double ActionSliderUI()
                {
                    return slider.value;
                }

                onValueChange?.SetValue(value, (Action<double>)ActionSlider);
                onUIChange?.SetValue(value, (Func<double>)ActionSliderUI);
                break;
        }
    }

    private void BindAction(MethodInfo method, object attr)
    {
        UIActionBind uiBind = (UIActionBind)attr;
        GObject obj = FguiUtils.GetUI<GObject>(main, uiBind._path);
        ParameterInfo[] methodParamsListClick;
        bool isAgent;

        switch (uiBind._type)
        {
            case UIAction.Click:
                var methodParamsClick = method.GetParameters();
                Delegate click;
                if (methodParamsClick.Length == 0)
                {
                    click = Delegate.CreateDelegate(typeof(EventCallback0), this, method);
                    obj.onClick.Set((EventCallback0)click);
                }
                else
                {
                    click = Delegate.CreateDelegate(typeof(EventCallback1), this, method);
                    obj.onClick.Set((EventCallback1)click);
                }

                break;
            case UIAction.ListRender:
                var render = Delegate.CreateDelegate(typeof(ListItemRenderer), this, method);
                obj.asList.itemRenderer = (ListItemRenderer)render;
                break;
            case UIAction.ListProvider:
                var provider = Delegate.CreateDelegate(typeof(ListItemProvider), this, method);
                obj.asList.itemProvider = (ListItemProvider)provider;
                break;
            case UIAction.ListClick:
                methodParamsListClick = method.GetParameters();
                Delegate listClick;
                if (methodParamsListClick.Length == 0)
                {
                    listClick = Delegate.CreateDelegate(typeof(EventCallback0), this, method);
                    obj.asList.onClickItem.Set((EventCallback0)listClick);
                }
                else
                {
                    listClick = Delegate.CreateDelegate(typeof(EventCallback1), this, method);
                    obj.asList.onClickItem.Set((EventCallback1)listClick);
                }

                break;
            case UIAction.DragStart:
                obj.draggable = true;
                isAgent = uiBind._extra.Length == 0 || uiBind._extra[0] == "Self";
                SetDragListener(obj, 0, method, isAgent);
                break;
            case UIAction.DragHold:
                obj.draggable = true;
                isAgent = uiBind._extra.Length == 0 || uiBind._extra[0] == "Self";
                SetDragListener(obj, 1, method, isAgent);
                break;
            case UIAction.DragEnd:
                obj.draggable = true;
                isAgent = uiBind._extra.Length == 0 || uiBind._extra[0] != "Self";
                SetDragListener(obj, 2, method, isAgent);
                break;
            case UIAction.Drop:
                Action<object> drop = (Action<object>)Delegate.CreateDelegate(typeof(Action<object>), this, method);

                _dropDic[obj.id] = true;

                EventManager.AddListening(obj.id, "OnDrop_" + obj.id, data => drop.Invoke(data));
                break;
        }
    }

    private void BindListener(MethodInfo method, object attr)
    {
        UIListenerBind uiBind = (UIListenerBind)attr;
        var eventFunc = Delegate.CreateDelegate(typeof(Action<ArrayList>), this, method);
        EventManager.AddListening(id, uiBind._name, (Action<ArrayList>)eventFunc);
    }

    private void ClearDropData()
    {
        _dropData.Clear();
    }

    /// <summary>
    /// 添加放置数据
    /// </summary>
    /// <param name="data"></param>
    protected void AddDropData(object data)
    {
        _dropData.Add(data);
    }

    /// <summary>
    /// 设置拖拽，用于list内元素
    /// </summary>
    /// <param name="type"></param>
    /// <param name="action"></param>
    /// <param name="obj"></param>
    protected void SetDrag(UIAction type, GObject obj, Action dragAction)
    {
        obj.draggable = true;
        Action action = () =>
        {
            //停止本次滚动
            obj.parent.asList.scrollPane.CancelDragging();
            dragAction();
        };
        switch (type)
        {
            case UIAction.DragStart:
                SetDragListener(obj, 0, action);
                break;
            case UIAction.DragHold:
                SetDragListener(obj, 1, action);
                break;
            case UIAction.DragEnd:
                SetDragListener(obj, 2, action);
                break;
        }
    }

    protected void SetDrop(GObject obj, Action<object> action)
    {
        _dropDic[obj.id] = true;
        EventManager.AddListening(obj.id, "OnDrop_" + obj.id, data => action(_dropData));
    }

    /// <summary>
    /// 添加放置数据
    /// </summary>
    /// <param name="datas"></param>
    protected void AddDropData(params object[] datas)
    {
        foreach (var data in datas)
        {
            _dropData.Add(data);
        }
    }

    /// <summary>
    /// 添加拖拽监听代理
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="type"></param>
    /// <param name="method"></param>
    /// <param name="isAgent"></param>
    private void SetDragListener(GObject obj, int type, MethodInfo method, bool isAgent)
    {
        ParameterInfo[] methodParamsListClick = method.GetParameters();

        var drag = Delegate.CreateDelegate(
            methodParamsListClick.Length == 0 ? typeof(EventCallback0) : typeof(EventCallback1), this, method);

        if (isAgent)
        {
            obj.onDragStart.Add(context =>
            {
                context.PreventDefault();
                //复制UI
                GameObject origin = obj.displayObject.gameObject;
                _copy = GameObject.Instantiate(origin, main.displayObject.gameObject.transform, true);
                CompClone(_copy.transform, origin.transform);

                //同步属性
                _copy.transform.localPosition = origin.transform.localPosition;
                _copy.transform.localScale = origin.transform.localScale;
                _copy.transform.localRotation = origin.transform.localRotation;

                //拖拽跟随逻辑
                _uiDrag = _copy.AddComponent<UIDrag>();
                _uiDrag.SetOriginMousePos();

                Action action = () =>
                {
                    //清除放置数据
                    ClearDropData();
                    if (methodParamsListClick.Length == 0)
                    {
                        ((EventCallback0)drag).Invoke();
                    }
                    else
                    {
                        ((EventCallback1)drag).Invoke(null);
                    }
                };

                switch (type)
                {
                    case 0:
                        _uiDrag.SetStart(action);
                        break;
                    case 1:
                        _uiDrag.SetUpdate(action);
                        break;
                    case 2:
                        _uiDrag.SetEnd(action);
                        break;
                }

                AddDropListener(obj);
                RemoveDragAgent();
            });
        }
        else
        {
            if (methodParamsListClick.Length == 0)
            {
                EventCallback0 action = () =>
                {
                    //清除放置数据
                    ClearDropData();
                    ((EventCallback0)drag).Invoke();
                };
                //监听鼠标拖拽
                switch (type)
                {
                    case 0:
                        obj.onDragStart.Set(action);
                        break;
                    case 1:
                        obj.onDragMove.Set(action);
                        break;
                    case 2:
                        obj.onDragEnd.Set(action);
                        break;
                }
            }
            else
            {
                EventCallback1 action = context =>
                {
                    //清除放置数据
                    ClearDropData();
                    ((EventCallback1)drag).Invoke(context);
                };
                //监听鼠标拖拽
                switch (type)
                {
                    case 0:
                        obj.onDragStart.Set(action);
                        break;
                    case 1:
                        obj.onDragMove.Set(action);
                        break;
                    case 2:
                        obj.onDragEnd.Set(action);
                        break;
                }
            }

            AddDropListener(obj);
        }
    }

    private void SetDragListener(GObject obj, int type, Action dragAction)
    {
        obj.onDragStart.Add(context =>
        {
            context.PreventDefault();
            //复制UI
            GameObject origin = obj.displayObject.gameObject;
            _copy = GameObject.Instantiate(origin, main.displayObject.gameObject.transform, true);
            CompClone(_copy.transform, origin.transform);

            //同步属性
            _copy.transform.position = origin.transform.position;
            _copy.transform.localScale = origin.transform.localScale;
            _copy.transform.rotation = origin.transform.rotation;

            //拖拽跟随逻辑
            _uiDrag = _copy.AddComponent<UIDrag>();
            _uiDrag.SetOriginMousePos();

            Action action = () =>
            {
                //清除放置数据
                ClearDropData();
                dragAction.Invoke();
            };

            switch (type)
            {
                case 0:
                    _uiDrag.SetStart(action);
                    break;
                case 1:
                    _uiDrag.SetUpdate(action);
                    break;
                case 2:
                    _uiDrag.SetEnd(action);
                    break;
            }

            AddDropListener(obj);
            RemoveDragAgent();
        });
    }

    /// <summary>
    /// 添加放置监听
    /// </summary>
    /// <param name="obj"></param>
    private void AddDropListener(GObject obj)
    {
        if (_uiDrag)
        {
            _uiDrag.AddEnd(() =>
            {
                GObject target = GRoot.inst.touchTarget;
                while (target != null)
                {
                    if (_dropDic.ContainsKey(target.id))
                    {
                        EventManager.TriggerEvent("OnDrop_" + target.id, _dropData);
                        return;
                    }

                    target = target.parent;
                }
            });
        }
        else
        {
            obj.onDragEnd.Add(() =>
            {
                GObject target = GRoot.inst.touchTarget;
                while (target != null)
                {
                    if (_dropDic.ContainsKey(target.id))
                    {
                        EventManager.TriggerEvent("OnDrop_" + target.id, _dropData);
                        return;
                    }

                    target = target.parent;
                }
            });
        }
    }

    /// <summary>
    /// 移除拖拽监听代理
    /// </summary>
    private void RemoveDragAgent()
    {
        if (_uiDrag)
        {
            _uiDrag.AddEnd(() =>
            {
                _copy = null;
                _uiDrag = null;
            });
        }
    }

    /// <summary>
    /// 代理组件克隆及处理
    /// </summary>
    /// <param name="transCopy"></param>
    /// <param name="transOrigin"></param>
    private void CompClone(Transform transCopy, Transform transOrigin)
    {
        MeshFilter filter = transCopy.GetComponent<MeshFilter>();
        MeshRenderer renderer = transCopy.GetComponent<MeshRenderer>();
        if (filter)
        {
            filter.mesh = transOrigin.GetComponent<MeshFilter>().mesh;
        }

        if (renderer)
        {
            // renderer.materials = transOrigin.GetComponent<MeshRenderer>().materials;
            Material[] origin = transOrigin.GetComponent<MeshRenderer>().materials;
            Material[] copy = new Material[origin.Length];
            for (int i = 0; i < origin.Length; i++)
            {
                copy[i] = new Material(origin[i]);
            }

            renderer.materials = copy;
            renderer.sortingOrder = 9999;
        }

        if (transCopy.childCount > 0)
        {
            for (int i = 0; i < transCopy.childCount; i++)
            {
                CompClone(transCopy.GetChild(i), transOrigin.GetChild(i));
            }
        }
    }
}