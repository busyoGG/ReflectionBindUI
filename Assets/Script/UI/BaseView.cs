using System;
using System.Reflection;
using FairyGUI;
using UnityEngine;

/// <summary>
/// UI�������
/// </summary>
public class BaseView : UIBase
{
    /// <summary>
    /// UI�ڵ�
    /// </summary>
    public UINode uiNode;

    /// <summary>
    /// ����������ʱ��
    /// </summary>
    private int _duration;

    /// <summary>
    /// ģ̬����
    /// </summary>
    private GGraph _model;

    /// <summary>
    /// �Ƿ񱣴�ڵ� Ĭ�ϱ���
    /// </summary>
    private bool _isSaveNode = true;

    public void OnAwake()
    {
        if (_isSaveNode)
        {
            //����ڵ�
            UIManager.Ins().SaveNode(name, uiNode);
        }

        //��UIԪ��
        Bind();

        //����
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
    /// չʾUI
    /// </summary>
    public void Show()
    {
        TweenIn();
        DoTween(true);
    }

    /// <summary>
    /// ����UI
    /// </summary>
    public void Hide()
    {
        TweenOut();
        DoTween(false);
    }

    /// <summary>
    /// ����UI
    /// </summary>
    public void Dispose()
    {
        main.Dispose();
    }

    /// <summary>
    /// ��ȡUI��ʾ���
    /// </summary>
    /// <returns></returns>
    public bool GetVisible()
    {
        return main.visible;
    }

    /// <summary>
    /// ����UI��ʾ���
    /// </summary>
    /// <param name="visible"></param>
    public void SetVisible(bool visible)
    {
        main.visible = visible;
    }

    /// <summary>
    /// ��ӻ���
    /// </summary>
    /// <param name="target">����Ŀ������</param>
    /// <param name="end">����Ŀ��ֵ</param>
    /// <param name="duration">����ʱ��</param>
    /// <param name="ease">��ֵ����</param>
    /// <param name="callback">�ص�</param>
    protected void AddTween(TweenTarget target, float end, int duration, TweenEaseType ease = TweenEaseType.Linear,
        Action callback = null)
    {
        UITweenManager.Ins().AddTween(main, target, end, duration, ease, callback);
        _duration = duration < _duration ? _duration : duration;
    }

    /// <summary>
    /// ��ӻ���
    /// </summary>
    /// <param name="target">����Ŀ������</param>
    /// <param name="end">����Ŀ��ֵ</param>
    /// <param name="duration">����ʱ��</param>
    /// <param name="ease">��ֵ����</param>
    /// <param name="callback">�ص�</param>
    protected void AddTween(TweenTarget target, Vector2 end, int duration, TweenEaseType ease = TweenEaseType.Linear,
        Action callback = null)
    {
        UITweenManager.Ins().AddTween(main, target, end, duration, ease, callback);
        _duration = duration < _duration ? _duration : duration;
    }

    /// <summary>
    /// ÿ��չʾ��ʱ��ִ��
    /// </summary>
    protected virtual void OnShow()
    {
    }

    /// <summary>
    /// ÿ�����ص�ʱ��ִ��
    /// </summary>
    protected virtual void OnHide()
    {
    }

    /// <summary>
    /// ����������ʼ������
    /// </summary>
    protected virtual void TweenIn()
    {
    }

    /// <summary>
    /// �˳�������ʼ������
    /// </summary>
    protected virtual void TweenOut()
    {
    }

    /// <summary>
    /// ���ó�ʼ��
    /// </summary>
    public virtual void InitConfig()
    {
    }

    /// <summary>
    /// ִ�л���
    /// </summary>
    /// <param name="start">�������˳�</param>
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
    /// �� ��
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
                    //����ģ̬����
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

                    //�����ĸ��¼�����֤��ק��ʵʱ�Ժ��ϸ���
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

                    //�����ö�
                    if (retop)
                    {
                        obj.onTouchBegin.Add(() => { UIManager.Ins().ResetTop(uiNode); });
                    }
                }
                else
                {
                    //������ק������Ҫ�л���ק״̬����˲������¼�
                    main.draggable = true;

                    //�����ö�
                    if (retop)
                    {
                        main.onTouchBegin.Set(() => { UIManager.Ins().ResetTop(uiNode); });
                    }
                }


                break;
        }
    }
}