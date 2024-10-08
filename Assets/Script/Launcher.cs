using UnityEngine;
using ReflectionUI;

public enum UIMouseState
{
    Up,
    Down,
    Hold
}

public class Launcher : MonoBehaviour
{
    private static UIMouseState _leftMouseState;

    private void Awake()
    {
        UITweenManager.Ins().Init();
    }

    // Start is called before the first frame update
    void Start()
    {
        
        _leftMouseState = UIMouseState.Up;

        UINode first = UIManager.Ins().ShowUI<TestView>("FGUI", "Test", "TestView0");
        UIManager.Ins().ShowUI<TestView>("FGUI", "Test", "TestView1");
        
        UIManager.Ins().DisposeUI("TestView1");
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
