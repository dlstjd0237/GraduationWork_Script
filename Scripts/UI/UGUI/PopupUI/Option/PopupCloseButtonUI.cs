using BIS.Events;
using BIS.Manager;
using Main.Runtime.Core.Events;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace BIS.UI.Popup
{
    public class PopupCloseButtonUI : OptionBtnUI
    {
        private GameEventChannelSO _uiEvent;

        private enum Texts
        {
            Text
        }

        public override bool Init()
        {
            if (base.Init() == false)
                return false;
            _uiEvent = Managers.Resource.Load<GameEventChannelSO>("UIEventChannelSO");
            _btn.onClick.AddListener(HandleClickEvent);
            BindTexts(typeof(Texts));
            return true;
        }

        private void HandleClickEvent()
        {
            _rootUI.BtnChoice("Close");
            SceneControlManager.FadeOut(() => Application.Quit());
            //OptionEvent evt = UIEvent.OptionEvent;
            //evt.isOpen = false;
            //_uiEvent.RaiseEvent(evt);
        }

        public override void SetUp(string btnName, GameObject contectBox, OptionPopupUI root)
        {
            _rootUI = root;
            var text = GetText((int)Texts.Text);

            text.SetText("EXIT");
        }

        public override void Choice(bool isChoice)
        {
            var text = GetText((int)Texts.Text);
            string textInfo = isChoice ? $"<b>Exit</b>" : "Exit";
            text.SetText(textInfo);
        }

        private void OnDestroy()
        {
            _btn.onClick.RemoveListener(HandleClickEvent);
        }
    }
}