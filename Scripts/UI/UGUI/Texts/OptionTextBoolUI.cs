using System;
using UnityEngine;

namespace BIS.UI
{
    public class OptionTextBoolUI : UIBase
    {
        [SerializeField] private string Key;
        [SerializeField] private bool _isValue = false;

        private enum Texts
        {
            ResultText
        }

        private enum Buttons
        {
            LeftArrow,
            RightArrow
        }

        public override bool Init()
        {

            if (base.Init() == false)
                return false;

            BindButtons(typeof(Buttons));
            BindTexts(typeof(Texts));

            GetButton((int)Buttons.LeftArrow).onClick.AddListener(HandleValueChange);
            GetButton((int)Buttons.RightArrow).onClick.AddListener(HandleValueChange);

            UpdateView();

            return true;
        }

        private void HandleValueChange()
        {
            _isValue = !_isValue;
            UpdateView();
        }


        public void UpdateView()
        {
            GetText((int)Texts.ResultText).SetText(_isValue == true ? "Yes" : "No");
        }
    }
}
