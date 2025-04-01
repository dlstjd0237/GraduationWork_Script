using BIS.Initializes;
using BIS.UI.Data;
using BIS.UI.MVVM;
using BIS.UIToolkit.ViewModel;
using UnityEngine;
using UnityEngine.UIElements;

namespace BIS.UIToolkit
{
    public class InGameViewModelUI : ViewModelBaseUI
    {
        private ProgressBarModelView _healthBar;
        public override bool Init()
        {
            if (base.Init() == false)
                return false;
            return true;
        }

        protected override void SetDataBindings()
        {
            Debug.Log(_root.Q<ProgressBar>("health_progressbar"));
            _healthBar = new ProgressBarModelView(new ProgressBarData(100, 0, 100, _root.Q<ProgressBar>("health_progressbar")));

        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                _healthBar.decreaseValue(50);
        }
    }
}
