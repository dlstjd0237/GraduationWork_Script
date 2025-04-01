using UnityEngine;
using UnityEngine.UIElements;

using BIS.Initializes;

using static BIS.Core.Utility.Util;

namespace BIS.UI.MVVM
{
    public abstract class ViewModelBaseUI : InitBase
    {
        [SerializeField] protected UIDocument _doc;
        public UIDocument Document { get => _doc; protected set => _doc = value; }

        protected VisualElement _root;

        protected const string HideKey = "Hide";
        protected const string ShowKey = "Show";


        public override bool Init()
        {
            if (base.Init() == false)
                return false;

            if (RegisterElements() == false)
                return false;

            SetDataBindings();



            return true;
        }

        protected virtual bool RegisterElements()
        {
            if (_doc == null)
                _doc = GetComponent<UIDocument>();

            _root = _doc.rootVisualElement;


            return _doc.ValueNullCheck();
        }

        protected abstract void SetDataBindings();


        private void OnDestroy()
        {
            Disconnect();
        }

        protected virtual void Disconnect()
        {

        }

    }
}
