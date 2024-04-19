using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UIMouseState
{
    Up,
    Down,
    Hold
}

public class Launcher : MonoBehaviour
{
    private static UIMouseState _leftMouseState;
    // Start is called before the first frame update
    void Start()
    {
        _leftMouseState = UIMouseState.Up;
        
        UIPackage.AddPackage("FGUI/Test");
        TestView testView = new TestView();
        testView.id = "ui_0";
        testView.main = UIPackage.CreateObject("Test","TestView").asCom;
        GRoot.inst.AddChild(testView.main);

        testView.OnAwake();
    }

    private void OnGUI()
    {
        if (_leftMouseState == UIMouseState.Up)
        {
            if (Input.GetMouseButton(0))
            {
                _leftMouseState = UIMouseState.Down;
            }
        }
        else if (_leftMouseState == UIMouseState.Down)
        {
            if (Input.GetMouseButton(0))
            {
                _leftMouseState = UIMouseState.Hold;
            }
            else
            {
                _leftMouseState = UIMouseState.Up;
            }
        }
        else
        {
            if (!Input.GetMouseButton(0))
            {
                _leftMouseState = UIMouseState.Up;
            }
        }
    }

    public static UIMouseState GetLeftMouseState()
    {
        return _leftMouseState;
    }
}
