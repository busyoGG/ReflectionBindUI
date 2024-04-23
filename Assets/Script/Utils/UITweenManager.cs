using System;
using FairyGUI;
using UnityEngine;

public class UITweenManager : Singleton<UITweenManager>
{
    private TweenUpdater _updater;

    private int _id = 0;

    public void Init()
    {
        GameObject obj = new GameObject();
        obj.name = "TweenUpdater";
        _updater = obj.AddComponent<TweenUpdater>();
    }

    public int AddTween(GObject obj, TweenTarget target, float end, int duration,
        TweenEaseType ease = TweenEaseType.Linear, Action callback = null)
    {
        UITween vt = new UITween();
        vt.id = _id++;
        vt.duration = duration;
        vt.updater = GenerateUpdater(obj, target, end, duration, ease);
        vt.callback = callback;

        _updater.AddTween(vt);

        return vt.id;
    }
    
    public int AddTween(GObject obj, TweenTarget target, Vector2 end, int duration,
        TweenEaseType ease = TweenEaseType.Linear, Action callback = null)
    {
        UITween vt = new UITween();
        vt.id = _id++;
        vt.duration = duration;
        vt.updater = GenerateUpdater(obj, target, end, duration, ease);
        vt.callback = callback;

        _updater.AddTween(vt);
        return vt.id;
    }

    public void StopTween(int id)
    {
        _updater.StopTween(id);
    }

    private Action<float> GenerateUpdater(GObject obj, TweenTarget target, float end, float duration,
        TweenEaseType ease)
    {
        void Action(float time)
        {
            float ratio = EaseUtil.Evaluate(ease, time, duration, 1.7f, 0);
            switch (target)
            {
                case TweenTarget.X:
                    obj.x = ratio * end;
                    break;
                case TweenTarget.Y:
                    obj.y = ratio * end;
                    break;
                case TweenTarget.ScaleX:
                    obj.scaleX = ratio * end;
                    break;
                case TweenTarget.ScaleY:
                    obj.scaleY = ratio * end;
                    break;
                case TweenTarget.Rotation:
                    obj.rotation = ratio * end;
                    break;
                case TweenTarget.Alpha:
                    obj.alpha = ratio * end;
                    break;
                case TweenTarget.Heihgt:
                    obj.height = ratio * end;
                    break;
                case TweenTarget.Width:
                    obj.width = ratio * end;
                    break;
            }
        }

        return Action;
    }
    
    private Action<float> GenerateUpdater(GObject obj, TweenTarget target, Vector2 end, float duration,
        TweenEaseType ease)
    {
        void Action(float time)
        {
            float ratio = EaseUtil.Evaluate(ease, time, duration, 1.7f, 0);
            switch (target)
            {
                case TweenTarget.Position:
                    obj.xy = ratio * end;
                    break;
                case TweenTarget.Scale:
                    obj.scale = ratio * end;
                    break;
                case TweenTarget.Size:
                    obj.size = ratio * end;
                    break;
            }
        }

        return Action;
    }
}