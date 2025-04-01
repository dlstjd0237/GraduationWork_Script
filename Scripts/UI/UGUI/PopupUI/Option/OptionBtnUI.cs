using System;

using UnityEngine;
using UnityEngine.UI;

using BIS.Core.Utility;

namespace BIS.UI.Popup
{
    public class OptionBtnUI : UIBase
    {
        private enum Texts
        {
            Text
        }

        private GameObject _contentUI;
        protected OptionPopupUI _rootUI;
        protected Button _btn;
        private string _key;

        public override bool Init()
        {
            if (base.Init() == false)
                return false;


            _btn = GetComponent<Button>();

            return true;
        }

        public virtual void SetUp(string btnName, GameObject contectBox, OptionPopupUI root)
        {
            gameObject.name = $"{btnName}_option_btn";

            BindTexts(typeof(Texts));
            GetText((int)Texts.Text).SetText(btnName.ToUpper());

            _contentUI = contectBox;
            _rootUI = root;
            _key = btnName;

            _btn.onClick.AddListener(HandleClickEvent);
        }

        private void HandleClickEvent()
        {
            _rootUI.BtnChoice(_key);
        }


        public virtual void Choice(bool isChoice)
        {
            Util.UIFadeOut(_contentUI, !isChoice);
            var text = GetText((int)Texts.Text);
            string textInfo = isChoice ? $"<b>{_key.ToUpper()}</b>" : _key.ToUpper();
            text.SetText(textInfo);
        }

        private void OnDestroy()
        {
            _btn.onClick.RemoveListener(HandleClickEvent);
        }
    }
}
