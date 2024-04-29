using System.Collections.Generic;

public class UINode
{
    public UINode parent;

    public int layer;

    public BaseView ui;

    public Dictionary<string, UINode> children = new Dictionary<string, UINode>();
}