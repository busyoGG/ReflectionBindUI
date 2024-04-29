using System;
using System.Reflection;
using FairyGUI;
using UnityEngine;

public class BaseView : UIBase
{
    public UINode uiNode;

    private int _duration;

    private GGraph _model;

    /// <summary>
    /// 只在创建的时候执行
    /// </summary>
    public void OnAwake()
    {
        Bind();

        var classAttributes = _type.GetCustomAttributes();

        foreach (var attr in classAttributes)
        {
            if (attr is UIClassBind)
            {
                BindClass(attr);
            }
        }
    }

    public void Show()
    {
        OnShow();
        TweenIn();
        DoTween(true);
    }

    public void Hide()
    {
        OnHide();
        TweenOut();
        DoTween(false);
    }

    public bool GetVisible()
    {
        return main.visible;
    }

    public void SetVisible(bool visible)
    {
        main.visible = visible;
    }

    protected void AddTween(TweenTarget target, float end, int duration, TweenEaseType ease = TweenEaseType.Linear,
        Action callback = null)
    {
        UITweenManager.Ins().AddTween(main, target, end, duration, ease, callback);
        _duration = duration < _duration ? _duration : duration;
    }

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


    protected virtual void LateOnShow()
    {
    }

    protected virtual void LateOnHide()
    {
    }

    protected virtual void TweenIn()
    {
    }

    protected virtual void TweenOut()
    {
    }

    private void DoTween(bool start)
    {
        if (start)
        {
            SetVisible(true);
            AddTween(TweenTarget.None, 0, _duration, TweenEaseType.Linear, LateOnShow);
        }
        else
        {
            AddTween(TweenTarget.None, 0, _duration, TweenEaseType.Linear, () =>
            {
                main.visible = false;
                LateOnHide();
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

                int index = GRoot.inst.GetChildIndex(main);
                GRoot.inst.AddChildAt(_model, index);

                _model.onClick.Set(() =>
                {
                    if (uiClassBind.extra.Length > 0 && uiClassBind.extra[0] == "hide")
                    {
                        Hide();
                        main.AddChild(_model);
                    }
                });

                break;
            case UIClass.Drag:
                bool retop = uiClassBind.extra[0] == "Retop";
                if (uiClassBind.extra.Length > 1)
                {
                    GObject obj = FguiUtils.GetUI<GObject>(main, uiClassBind.extra[1]);
                    obj.draggable = true;
                    bool isTouch = false;
                    bool isOut = true;

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
                    
                    if (retop)
                    {
                        obj.onTouchBegin.Add(() =>
                        {
                            UIManager.Ins().ResetTop(uiNode);
                        });
                    }
                }
                else
                {
                    main.draggable = true;
                
                    if (retop)
                    {
                        main.onTouchBegin.Set(() =>
                        {
                            UIManager.Ins().ResetTop(uiNode);
                        });
                    }
                }


                break;
        }
    }
}