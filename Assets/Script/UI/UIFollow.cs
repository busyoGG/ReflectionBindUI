using System.Collections;
using System.Collections.Generic;
using FairyGUI;
using UnityEngine;

public class UIFollow : MonoBehaviour
{
    private GObject _obj;

    private GObject _parent;

    void Update()
    {
        if (_obj.visible)
        {
            _obj.xy = FguiUtils.GetMousePosition(_parent);
        }
    }

    public void SetObj(GObject obj,GObject parent)
    {
        _obj = obj;
        _parent = parent;
    }
}