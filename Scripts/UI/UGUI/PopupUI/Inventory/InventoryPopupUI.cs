using UnityEngine;
using PJH.Players;
using BIS.Events;
using Main.Runtime.Core.Events;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using BIS.Shared;
using BIS.Core.Utility;
using System.Collections;
using BIS.Manager;

namespace BIS.UI.Popup
{
    public class InventoryPopupUI : PopupUI
    {
        private readonly Color _defualtButtonColor = new Color(0.7137255f, 0.7176471f, 0.7803922f);

        private GameEventChannelSO _uiEventChannelSO;
        [SerializeField] private PlayerInputSO _inputSO;

        private enum Buttons
        {
            ExitButton
        }

        public override bool Init()
        {
            if (base.Init() == false)
                return false;
            _uiEventChannelSO = Managers.Resource.Load<GameEventChannelSO>("UIEventChannelSO");
            _uiEventChannelSO.AddListener<InventoryEvent>(HandleInventoryEvent);

            BindButtons((typeof(Buttons)));

            ExitButtonSetting();

            _inputSO.ESCEvent += HandleExitClickEvent;

            return true;
        }


        #region ExitSetting

        private void ExitButtonSetting()
        {
            Button btn = GetButton((int)Buttons.ExitButton);

            btn.onClick.AddListener(HandleExitClickEvent);
            BindEvent(btn.gameObject, HandleExitPointerEnter, EUIEvent.PointerEnter);
            BindEvent(btn.gameObject, HandleEnterPointerExit, EUIEvent.PointerExit);
        }

        private void HandleExitClickEvent()
        {
            UIEvent.InventoryEvent.isOpen = false;
            _uiEventChannelSO.RaiseEvent(UIEvent.InventoryEvent);
        }

        private void HandleEnterPointerExit(PointerEventData obj)
        {
            GameObject go = GetButton((int)Buttons.ExitButton).gameObject;
            Util.UIColorChange<Image>(go, _defualtButtonColor);
        }

        private void HandleExitPointerEnter(PointerEventData obj)
        {
            GameObject go = GetButton((int)Buttons.ExitButton).gameObject;
            Util.UIColorChange<Image>(go, Color.white);
        }

        #endregion

        private void HandleInventoryEvent(InventoryEvent evt)
        {
            if (evt.isOpen)
            {
                _inputSO.EnablePlayerInput(false);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                OpenPopup();
            }
            else
            {
                _inputSO.EnablePlayerInput(true);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                ClosePopup();
            }
        }

        private void OnDestroy()
        {
            _inputSO.ESCEvent -= HandleExitClickEvent;
            _uiEventChannelSO.RemoveListener<InventoryEvent>(HandleInventoryEvent);
        }
    }
}