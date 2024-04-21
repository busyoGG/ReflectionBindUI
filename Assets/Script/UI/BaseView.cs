public class BaseView : UIBase
{
    /// <summary>
    /// 只在创建的时候执行
    /// </summary>
    public void OnAwake()
    {
        Bind();
    }

    public void Show()
    {
        main.visible = true;
        OnShow();
    }

    public void Hide()
    {
        main.visible = false;
        OnHide();
    }

    public bool GetVisible()
    {
        return main.visible;
    }

    /// <summary>
    /// 每次展示的时候执行
    /// </summary>
    protected virtual void OnShow() { }

    /// <summary>
    /// 每次隐藏的时候执行
    /// </summary>
    protected virtual void OnHide() { }
}
