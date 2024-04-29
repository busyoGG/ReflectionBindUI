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
            ConsoleUtils.Log("开始拖拽");
            AddDropData(index);
        });

        SetDrop(comp, (object data) => { ConsoleUtils.Log("放置", data); });
    }

    [UIActionBind(UIAction.Click, "3")]
    private void OnBtnClick()
    {
        ConsoleUtils.Log("点击了按钮");
        _comboBoxIndex.Set(1);
        EventManager.TriggerEvent("show_console", null);
    }

    [UIActionBind(UIAction.DragEnd, "3", "Self")]
    private void OnDrag(EventContext context)
    {
        //添加拖拽数据
        AddDropData(1, 2, "测试数据");
        Debug.Log("结束拖拽");
        Debug.Log(GRoot.inst.touchTarget);
    }

    [UIActionBind(UIAction.Drop, "4")]
    private void OnDrop(object data)
    {
        ConsoleUtils.Log("拖拽放置", data);
    }

    [UIListenerBind("show_console")]
    private void ShowConsole(ArrayList data)
    {
        ConsoleUtils.Log("触发了事件");
        _loaderUrl.Set("ui://Test/Icon");
        _testList.Set(new List<string> { "a", "b", "c" });

        // _slideValue.Set(70);
        ConsoleUtils.Log(_slideValue.Get(), _input.Get());
    }

    [UIActionBind(UIAction.Hover,"7")]
    private void ShowHint()
    {
        ShowFloatView<HintView>("input_hint","HintView",true);
        // ConsoleUtils.Log("显示悬浮窗");
    }

    [UIActionBind(UIAction.Slider,"5")]
    private void OnSliderChanged()
    {
        ConsoleUtils.Log("滑动条",_slideValue.Get());
    }
    
    [UIActionBind(UIAction.ComboBox,"9")]
    private void OnComboBoxChanged()
    {
        ConsoleUtils.Log("下拉框",_comboBoxIndex.Get());
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