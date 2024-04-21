public class BaseView : UIBase
{
    /// <summary>
    /// ֻ�ڴ�����ʱ��ִ��
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
    /// ÿ��չʾ��ʱ��ִ��
    /// </summary>
    protected virtual void OnShow() { }

    /// <summary>
    /// ÿ�����ص�ʱ��ִ��
    /// </summary>
    protected virtual void OnHide() { }
}
