using System;
using UnityEngine;

public class BaseView : UIBase
{
    private int _duration;

    /// <summary>
    /// 只在创建的时候执行
    /// </summary>
    public void OnAwake()
    {
        Bind();
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
            AddTween(TweenTarget.None,0,_duration,TweenEaseType.Linear,LateOnShow);
        }
        else
        {
            AddTween(TweenTarget.None,0,_duration,TweenEaseType.Linear, () =>
            {
                main.visible = false;
                LateOnHide();
            });
        }
    }
}