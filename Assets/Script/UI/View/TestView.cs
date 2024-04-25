using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestView : BaseView
{
    [UIDataBind(UIType.TextField, "0")]
    private StringUIProp _testText { get; set; }

    [UIDataBind(UIType.List, "n1")]
    private UIListProp<string> _testList { get; set; }

    [UIDataBind(UIType.Loader, "3")]
    private StringUIProp _loaderUrl { get; set; }

    [UIDataBind(UIType.Slider, "4")]
    private DoubleUIProp _slideValue { get; set; }

    [UIDataBind(UIType.TextInput, "5")]
    private StringUIProp _input { get; set; }

    [UICompBind(UIType.Loader, "3")]
    private GLoader _loader { get; set; }


    [UIActionBind(UIAction.ListRender, "1")]
    private void ItemRenderer(int index, GObject item)
    {
        GComponent comp = item as GComponent;
        GTextField content = comp.GetChildAt(0).asTextField;

        content.text = _testList.Get()[index];

        SetDrag(UIAction.DragStart, comp, () =>
        {
            ConsoleUtils.Log("��ʼ��ק");
            AddDropData(index);
        });

        SetDrop(comp, (object data) => { ConsoleUtils.Log("����", data); });
    }

    [UIActionBind(UIAction.Click, "2")]
    private void OnBtnClick()
    {
        ConsoleUtils.Log("����˰�ť");
        EventManager.TriggerEvent("show_console", null);
    }

    [UIActionBind(UIAction.DragEnd, "2", "Self")]
    private void OnDrag(EventContext context)
    {
        //�����ק����
        AddDropData(1, 2, "��������");
        Debug.Log("������ק");
        Debug.Log(GRoot.inst.touchTarget);
    }

    [UIActionBind(UIAction.Drop, "3")]
    private void OnDrop(object data)
    {
        ConsoleUtils.Log("��ק����", data);
    }

    [UIListenerBind("show_console")]
    private void ShowConsole(ArrayList data)
    {
        ConsoleUtils.Log("�������¼�");
        _loaderUrl.Set("ui://Test/Icon");
        _testList.Set(new List<string> { "a", "b", "c" });

        // _slideValue.Set(70);
        ConsoleUtils.Log(_slideValue.Get(), _input.Get());
    }

    [UIActionBind(UIAction.Hover,"6")]
    private void ShowHint()
    {
        ShowFloatView<HintView>("input_hint","HintView",true);
        ConsoleUtils.Log("��ʾ������");
    }

    // protected override void TweenIn()
    // {
    //     main.x = -500;
    //     AddTween(TweenTarget.X,500,2000,TweenEaseType.CircOut);
    //     
    //     main.y = 500;
    //     AddTween(TweenTarget.Y,-500,2000,TweenEaseType.Linear);
    //     
    //     main.scaleX = 0.5f;
    //     AddTween(TweenTarget.ScaleX,0.5f,2000,TweenEaseType.CircOut);
    //     
    //     AddTween(TweenTarget.Rotation,50,2000,TweenEaseType.CircOut);
    // }
    //
    // protected override void LateOnShow()
    // {
    //     ConsoleUtils.Log("�����ص�",Time.time);
    // }

    // protected override void TweenOut()
    // {
    //     AddTween(TweenTarget.X,500,2000,TweenEaseType.CircOut);
    // }
}