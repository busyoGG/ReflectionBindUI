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
        ConsoleUtils.Log("����˰�ť");
        EventManager.TriggerEvent("show_console", null);
    }

    [UIActionBind("DragEnd","2")]
    private void OnDrag(EventContext context)
    {
        //�����ק����
        AddDropData(1,2,"��������");
        Debug.Log("������ק");
        Debug.Log(GRoot.inst.touchTarget);
    }
    
    [UIActionBind("Drop","3")]
    private void OnDrop(object data)
    {
        ConsoleUtils.Log("��ק����",data);
    }

    [UIListenerBind("show_console")]
    private void ShowConsole(ArrayList data)
    {
        ConsoleUtils.Log("�������¼�");
        _loaderUrl.Set("ui://Test/Icon");
        _testList.Set(new List<string> { "a", "b", "c" });
    }
}
