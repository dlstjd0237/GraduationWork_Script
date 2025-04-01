using BIS.Data;
using BIS.Manager;
using BIS.Shared;
using System;
using UnityEngine;

namespace BIS.UI.Popup
{
    public class InformationPopupUI : PopupUI
    {
        public event Action CloseEvent;
        private enum Texts
        {
            Title_Text,
            Description_Text
        }
        private enum Buttons
        {
            Close_Btn
        }

        public override bool Init()
        {
            if (base.Init() == false)
                return false;
            Managers.UI.SetCanvas(gameObject, false, 30);

            // ==== Bind UI ====
            BindTexts(typeof(Texts));
            BindButtons(typeof(Buttons));
            // =================

            BindEvent(GetButton((int)Buttons.Close_Btn).gameObject, (evt) => ClosePopup(CloseEvent), EUIEvent.Click); return true;
        }

        public void SetUpUI(InformationSO so)
        {
            GetText((int)Texts.Title_Text).text = so.TitleInfo;
            GetText((int)Texts.Description_Text).text = so.DescriptionInfo;
        }
    }
}
