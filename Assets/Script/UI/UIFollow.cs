using FairyGUI;
using UnityEngine;

namespace ReflectionUI
{
    public class UIFollow : MonoBehaviour
    {
        private GObject _obj;

        private GObject _parent;

        void Update()
        {
            if (_obj.visible)
            {
                _obj.xy = FGUIUtils.GetMousePosition(_parent);
            }
        }

        public void SetObj(GObject obj, GObject parent)
        {
            _obj = obj;
            _parent = parent;
        }
    }
}