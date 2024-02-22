using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UIPackage.AddPackage("FGUI/Test");
        TestView testView = new TestView();
        testView.id = "ui_0";
        testView.main = UIPackage.CreateObject("Test","TestView").asCom;
        GRoot.inst.AddChild(testView.main);

        testView.OnAwake();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
