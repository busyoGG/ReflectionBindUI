using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestView : BaseView
{
    [UIDataBind("TextField","0")]
    private UIProp _testText {  get; set; }
    [UIDataBind("List","n1")]
    private UIListProp<string> _testList { get; set; }
    [UIDataBind("Loader","3")]
    private UIProp _loaderUrl { get; set; }

    [UICompBind("Loader","3")]
    private GLoader _loader { get; set; }


    [UIActionBind("ListRender","1")]
    private void ItemRenderer(int index,GObject item)
    {
        GComponent comp = item as GComponent;
        GTextField content = comp.GetChildAt(0).asTextField;

        content.text = _testList.Get()[index];
    }

    [UIActionBind("Click","2")]
    private void OnBtnClick()
    {
        ConsoleUtils.Log("点击了按钮");
        EventManager.TriggerEvent("show_console", null);
    }

    [UIActionBind("DragEnd","2")]
    private void OnDrag(EventContext context)
    {
        //添加拖拽数据
        AddDropData(1,2,"测试数据");
        Debug.Log("结束拖拽");
        Debug.Log(GRoot.inst.touchTarget);
    }
    
    [UIActionBind("Drop","3")]
    private void OnDrop(object data)
    {
        ConsoleUtils.Log("拖拽放置",data);
    }

    [UIListenerBind("show_console")]
    private void ShowConsole(ArrayList data)
    {
        ConsoleUtils.Log("触发了事件");
        _loaderUrl.Set("ui://Test/Icon");
        _testList.Set(new List<string> { "a", "b", "c" });
    }
}
