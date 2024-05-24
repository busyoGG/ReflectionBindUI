using UnityEngine;
using FairyGUI;

public class FGUIUtils
{
    /// <summary>
    /// 根据路径获取UI组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="comp"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    public static T GetUI<T>(GComponent comp, string path) where T : GObject
    {
        string[] paths = path.Split('/');
        GObject res = null;
        GComponent parent = comp;
        foreach (string s in paths)
        {
            if (s == "") continue;
            int output;
            bool isNumeric = int.TryParse(s, out output);
            if (isNumeric)
            {
                res = parent.GetChildAt(output);
            }
            else
            {
                res = parent.GetChild(s);
            }
            if (res == null)
            {
                ConsoleUtils.Error("ui路径错误", path);
                return null;
            }
            if (res is GComponent)
            {
                parent = res.asCom;
            }
            else
            {
                break;
            }
        }
        return res as T;
    }

    /// <summary>
    /// 获取鼠标在FGUI的坐标
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static Vector2 GetMousePosition(GObject obj = null)
    {
        //鼠标在FGUI的坐标
        Vector2 pos = Stage.inst.GetTouchPosition(-1);
        Vector2 logicPos;
        if (obj == null)
        {
            //鼠标屏幕位置转换为FGUI逻辑位置
            logicPos = GRoot._inst.GlobalToLocal(pos);
        }
        else
        {
            logicPos = obj.GlobalToLocal(pos);
        }
        return logicPos;
    }
}
