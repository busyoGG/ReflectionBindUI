using System;
using System.Reflection;
using FairyGUI;
using UnityEngine;

/// <summary>
/// UI界面基类
/// </summary>
public class BaseView : UIBase
{
    /// <summary>
    /// UI节点
    /// </summary>
    public UINode uiNode;

    /// <summary>
    /// 缓动最大持续时间
    /// </summary>
    private int _duration;

    /// <summary>
    /// 模态背景
    /// </summary>
    private GGraph _model;

    /// <summary>
    /// 是否保存节点 默认保存
    /// </summary>
    private bool _isSaveNode = true;

    public void OnAwake()
    {
        if (_isSaveNode)
        {
            //保存节点
            UIManager.Ins().SaveNode(name, uiNode);
        }

        //绑定UI元素
        Bind();

        //绑定类
        var classAttributes = _type.GetCustomAttributes();

        foreach (var attr in classAttributes)
        {
            if (attr is UIClassBind)
            {
                BindClass(attr);
            }
        }
    }

    /// <summary>
    /// 展示UI
    /// </summary>
    public void Show()
    {
        TweenIn();
        DoTween(true);
    }

    /// <summary>
    /// 隐藏UI
    /// </summary>
    public void Hide()
    {
        TweenOut();
        DoTween(false);
    }

    /// <summary>
    /// 销毁UI
    /// </summary>
    public void Dispose()
    {
        main.Dispose();
    }

    /// <summary>
    /// 获取UI显示情况
    /// </summary>
    /// <returns></returns>
    public bool GetVisible()
    {
        return main.visible;
    }

    /// <summary>
    /// 设置UI显示情况
    /// </summary>
    /// <param name="visible"></param>
    public void SetVisible(bool visible)
    {
        main.visible = visible;
    }

    /// <summary>
    /// 添加缓动
    /// </summary>
    /// <param name="target">缓动目标属性</param>
    /// <param name="end">缓动目标值</param>
    /// <param name="duration">持续时间</param>
    /// <param name="ease">插值函数</param>
    /// <param name="callback">回调</param>
    protected void AddTween(TweenTarget target, float end, int duration, TweenEaseType ease = TweenEaseType.Linear,
        Action callback = null)
    {
        UITweenManager.Ins().AddTween(main, target, end, duration, ease, callback);
        _duration = duration < _duration ? _duration : duration;
    }

    /// <summary>
    /// 添加缓动
    /// </summary>
    /// <param name="target">缓动目标属性</param>
    /// <param name="end">缓动目标值</param>
    /// <param name="duration">持续时间</param>
    /// <param name="ease">插值函数</param>
    /// <param name="callback">回调</param>
    protected void AddTween(TweenTarget target, Vector2 end, int duration, TweenEaseType ease = TweenEaseType.Linear,
        Action callback = null)
    {
        UITweenManager.Ins().AddTween(main, target, end, duration, ease, callback);
        _duration = duration < _duration ? _duration : duration;
    }

    /// <summary>
    /// 每次展示的时候执行
    /// </summary>
    protected virtual void OnShow()
    {
    }

    /// <summary>
    /// 每次隐藏的时候执行
    /// </summary>
    protected virtual void OnHide()
    {
    }

    /// <summary>
    /// 进场缓动初始化方法
    /// </summary>
    protected virtual void TweenIn()
    {
    }

    /// <summary>
    /// 退场缓动初始化方法
    /// </summary>
    protected virtual void TweenOut()
    {
    }

    /// <summary>
    /// 配置初始化
    /// </summary>
    public virtual void InitConfig()
    {
    }

    /// <summary>
    /// 执行缓动
    /// </summary>
    /// <param name="start">进场或退场</param>
    private void DoTween(bool start)
    {
        if (start)
        {
            SetVisible(true);
            AddTween(TweenTarget.None, 0, _duration, TweenEaseType.Linear, OnShow);
        }
        else
        {
            AddTween(TweenTarget.None, 0, _duration, TweenEaseType.Linear, () =>
            {
                main.visible = false;
                OnHide();
            });
        }
    }

    /// <summary>
    /// 绑定 类
    /// </summary>
    /// <param name="attr"></param>
    private void BindClass(object attr)
    {
        UIClassBind uiClassBind = (UIClassBind)attr;

        switch (uiClassBind.type)
        {
            case UIClass.Model:
                if (_model == null)
                {
                    //创建模态背景
                    _model = new GGraph();
                    _model.displayObject.gameObject.name = id + "_" + name + "_Model";

                    UIColor colorAttr = _type.GetCustomAttribute<UIColor>();
                    Color color = new Color(0, 0, 0, 0);
                    if (colorAttr != null)
                    {
                        color = colorAttr.color;
                    }

                    Vector2 size = GRoot.inst.size;
                    _model.DrawRect(size.x, size.y, 0, new Color(), color);
                }

                UIManager.Ins().SetModel(uiNode, _model);

                if (uiClassBind.extra.Length > 0 && uiClassBind.extra[0] == "Hide")
                {
                    _model.onClick.Set(() =>
                    {
                        Hide();
                        main.AddChild(_model);
                    });
                }

                break;
            case UIClass.Drag:
                bool retop = uiClassBind.extra.Length > 0 && uiClassBind.extra[0] == "Retop";
                if (uiClassBind.extra.Length > 1)
                {
                    GObject obj = FguiUtils.GetUI<GObject>(main, uiClassBind.extra[1]);
                    obj.draggable = true;
                    bool isTouch = false;
                    bool isOut = true;

                    //监听四个事件，保证拖拽的实时性和严格性
                    obj.onTouchBegin.Set(() =>
                    {
                        main.draggable = true;
                        isTouch = true;
                    });

                    obj.onTouchEnd.Set(() =>
                    {
                        if (isOut)
                        {
                            main.draggable = false;
                        }

                        isTouch = false;
                    });

                    obj.onRollOver.Set(() =>
                    {
                        main.draggable = true;
                        isOut = false;
                    });

                    obj.onRollOut.Set(() =>
                    {
                        if (!isTouch)
                        {
                            main.draggable = false;
                        }

                        isOut = true;
                    });

                    //监听置顶
                    if (retop)
                    {
                        obj.onTouchBegin.Add(() => { UIManager.Ins().ResetTop(uiNode); });
                    }
                }
                else
                {
                    //整体拖拽，不需要切换拖拽状态，因此不监听事件
                    main.draggable = true;

                    //监听置顶
                    if (retop)
                    {
                        main.onTouchBegin.Set(() => { UIManager.Ins().ResetTop(uiNode); });
                    }
                }


                break;
        }
    }
}