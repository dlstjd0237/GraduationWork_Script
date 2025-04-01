using BIS.Events;
using BIS.Manager;
using BIS.UI.Popup;
using Main.Runtime.Core.Events;
using PJH.Players;
using UnityEngine;

namespace BIS.UI.Scenes
{
    public class BattleSceneUI : SceneUI
    {
        private GameEventChannelSO _uiEventChannelSO;
        [SerializeField] private PlayerInputSO _inputSO;

        private void Awake()
        {
            _uiEventChannelSO = Managers.Resource.Load<GameEventChannelSO>("UIEventChannelSO");

            _uiEventChannelSO.AddListener<OptionEvent>(HandleOptionOpenEvent);

            _inputSO.ESCEvent += HandleOptionEvent;
        }

        private void HandleOptionEvent()
        {
            UIEvent.OptionEvent.isOpen = !UIEvent.OptionEvent.isOpen;
            _uiEventChannelSO.RaiseEvent(UIEvent.OptionEvent);
        }


        private void HandleOptionOpenEvent(OptionEvent evt)
        {
            if (evt.isOpen)
            {
                Time.timeScale = 0;
                _inputSO.EnablePlayerInput(false);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Managers.UI.ShowPopup<OptionPopupUI>("OptionPopupUI");
            }
            else
            {
                Time.timeScale = 1;
                _inputSO.EnablePlayerInput(true);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Managers.UI.ClosePopupUI();
            }
        }


        private void OnDestroy()
        {
            _uiEventChannelSO.RemoveListener<OptionEvent>(HandleOptionOpenEvent);
            _inputSO.ESCEvent -= HandleOptionEvent;
        }
    }
}