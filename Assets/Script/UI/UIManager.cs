using FairyGUI;

public class UIManager : Singleton<UIManager>
{
    private UINode _root;

    private int _id = 0;

    public void Init()
    {
        _root = new UINode();
    }

    public UINode ShowUI<T>(string folder, string package, string uiName, UINode parent = null) where T : BaseView, new()
    {
        string packagePath = folder + "/" + package;
        UIPackage.AddPackage(packagePath);
        //创建UI
        T view = new T();
        view.id = "ui_" + _id++;
        view.main = UIPackage.CreateObject(package, uiName).asCom;
        GRoot.inst.AddChild(view.main);

        view.OnAwake();
        view.Show();

        //创建UI节点
        UINode ui = new UINode();
        ui.ui = view;

        if (parent != null)
        {
            parent.children.Add(view.id, ui);
            ui.parent = parent;
        }
        else
        {
            _root.children.Add(view.id, ui);
            ui.parent = _root;
        }

        view.uiNode = ui;

        return ui;
    }

    public void HideUI(UINode ui)
    {
        foreach (var child in ui.children)
        {
            UINode uiChild = child.Value;
            HideUI(uiChild);
        }

        ui.ui.Hide();
    }

    public UINode GetUI(string name, UINode parent = null)
    {
        if (parent == null)
        {
            parent = _root;
        }

        if (name == parent.ui.name)
        {
            return parent;
        }

        foreach (var child in parent.children)
        {
            GetUI(name, child.Value);
        }

        return null;
    }
}