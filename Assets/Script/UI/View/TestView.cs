using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[UIClassBind(UIClass.Drag,"Retop","n10"),UIColor(1,1,1,0.5f)]
public class TestView : BaseView
{
    [UIDataBind(UIType.TextField, "1")]
    private StringUIProp _testText { get; set; }

    [UIDataBind(UIType.List, "n1")]
    private UIListProp<string> _testList { get; set; }

    [UIDataBind(UIType.Loader, "4")]
    private StringUIProp _loaderUrl { get; set; }

    [UIDataBind(UIType.Slider, "5")]
    private DoubleUIProp _slideValue { get; set; }

    [UIDataBind(UIType.TextInput, "6")]
    private StringUIProp _input { get; set; }
    
    [UIDataBind(UIType.ComboBox,"9","1","b")]
    private DoubleUIProp _comboBoxIndex { get; set; }

    [UICompBind(UIType.Loader, "4")]
    private GLoader _loader { get; set; }


    [UIActionBind(UIAction.ListRender, "2")]
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

    [UIActionBind(UIAction.Click, "3")]
    private void OnBtnClick()
    {
        ConsoleUtils.Log("����˰�ť");
        _comboBoxIndex.Set(1);
        EventManager.TriggerEvent("show_console", null);
    }

    [UIActionBind(UIAction.DragEnd, "3", "Self")]
    private void OnDrag(EventContext context)
    {
        //�����ק����
        AddDropData(1, 2, "��������");
        Debug.Log("������ק");
        Debug.Log(GRoot.inst.touchTarget);
    }

    [UIActionBind(UIAction.Drop, "4")]
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

    [UIActionBind(UIAction.Hover,"7")]
    private void ShowHint()
    {
        ShowFloatView<HintView>("input_hint","HintView",true);
        // ConsoleUtils.Log("��ʾ������");
    }

    [UIActionBind(UIAction.Slider,"5")]
    private void OnSliderChanged()
    {
        ConsoleUtils.Log("������",_slideValue.Get());
    }
    
    [UIActionBind(UIAction.ComboBox,"9")]
    private void OnComboBoxChanged()
    {
        ConsoleUtils.Log("������",_comboBoxIndex.Get());
    }

    [UIActionBind(UIAction.Click,"n11")]
    private void Close()
    {
        UIManager.Ins().HideUI(uiNode);
        // UIManager.Ins().DisposeUI(uiNode);
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
    protected override void OnShow()
    {
        ConsoleUtils.Log("OnShow",Time.time);
    }

    protected override void OnHide()
    {
        ConsoleUtils.Log("OnHide");
    }

    // protected override void TweenOut()
    // {
    //     AddTween(TweenTarget.X,500,2000,TweenEaseType.CircOut);
    // }
}