using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestView : BaseView
{
    [UIDataBind(UIType.TextField, "0")] private StringUIProp _testText { get; set; }

    [UIDataBind(UIType.List, "n1")] private UIListProp<string> _testList { get; set; }

    [UIDataBind(UIType.Loader, "3")] private StringUIProp _loaderUrl { get; set; }

    [UIDataBind(UIType.Slider, "4")] private DoubleUIProp _slideValue { get; set; }

    [UIDataBind(UIType.TextInput, "5")] private StringUIProp _input { get; set; }

    [UICompBind(UIType.Loader, "3")] private GLoader _loader { get; set; }


    [UIActionBind(UIAction.ListRender, "1")]
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
        
        SetDrop(comp, (object data) =>
        {
            ConsoleUtils.Log("放置",data);
        });
    }

    [UIActionBind(UIAction.Click, "2")]
    private void OnBtnClick()
    {
        ConsoleUtils.Log("点击了按钮");
        EventManager.TriggerEvent("show_console", null);
    }

    [UIActionBind(UIAction.DragEnd, "2", "Self")]
    private void OnDrag(EventContext context)
    {
        //添加拖拽数据
        AddDropData(1, 2, "测试数据");
        Debug.Log("结束拖拽");
        Debug.Log(GRoot.inst.touchTarget);
    }

    [UIActionBind(UIAction.Drop, "3")]
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
}