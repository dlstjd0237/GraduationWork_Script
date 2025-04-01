using BIS.Events;
using BIS.Manager;
using Main.Runtime.Core.Events;
using PJH.Players;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BIS.UI.Popup
{
    public class OptionPopupUI : PopupUI
    {
        private enum Objects
        {
            Button_Box_Contain,
            Option_Contain_Box
        }


        private Transform _btnBoxTrm;
        private Transform _infoBoxTrm;

        [SerializeField] private List<string> _optionInfoName;
        [SerializeField] private PlayerInputSO _playerInput;
        private Dictionary<string, OptionBtnUI> _optionBtnDictionary;

        public override bool Init()
        {
            if (base.Init() == false)
                return false;

            _optionBtnDictionary = new Dictionary<string, OptionBtnUI>();


            //==== UI Bind ====
            BindObjects(typeof(Objects));

            _btnBoxTrm = GetObject((int)Objects.Button_Box_Contain).transform;
            _infoBoxTrm = GetObject((int)Objects.Option_Contain_Box).transform;
            //=================


            for (int i = 0; i < _optionInfoName.Count; ++i)
            {
                string infoName = _optionInfoName[i];

                GameObject info = Managers.Resource.Instantiate($"{infoName}_Box_Contain", _infoBoxTrm);
                GameObject btn = Managers.Resource.Instantiate("Option_Btn", _btnBoxTrm);
                OptionBtnUI optionBtn = btn.GetComponent<OptionBtnUI>();

                optionBtn.SetUp(infoName, info, this);

                _optionBtnDictionary.Add(infoName, optionBtn);
            }

            GameObject closeBtn = Managers.Resource.Instantiate("Option_Close_Btn", _btnBoxTrm);
            PopupCloseButtonUI closeOptionBtn = closeBtn.GetComponent<PopupCloseButtonUI>();
            closeOptionBtn.SetUp("Close", null, this);

            _optionBtnDictionary.Add("Close", closeOptionBtn);

            _optionBtnDictionary[_optionInfoName[0]].Choice(true);
            return true;
        }


        public override void ClosePopup(Action callBack = null)
        {
            for (int i = 0; i < _optionInfoName.Count; ++i)
            {
                _optionBtnDictionary[_optionInfoName[i]].Choice(false);
            }

            base.ClosePopup(callBack);
        }

        public void BtnChoice(string btnName)
        {
            bool isChoice = false;
            for (int i = 0; i < _optionInfoName.Count; ++i)
            {
                isChoice = btnName == _optionInfoName[i] ? true : false;
                _optionBtnDictionary[_optionInfoName[i]].Choice(isChoice);
            }
        }
    }
}