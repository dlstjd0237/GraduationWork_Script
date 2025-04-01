using BIS.Data;
using BIS.Events;
using BIS.Manager;
using BIS.UI.Popup;
using Main.Runtime.Core.Events;
using Main.Shared;
using System;
using UnityEngine;

using Random = UnityEngine.Random;

namespace BIS.Interactions
{
    public class Trader : MonoBehaviour, IInteractable
    {
        public Transform UIDisplayTrm => transform;
        public Vector3 AdditionalUIDisplayPos => Vector3.up * 1;
        [field: SerializeField] public string Description { get; set; }
        private GameEventChannelSO _uiEventChannelSO;
        [SerializeField] private DialogueSO[] _currentTextDatas;
        [SerializeField] private DialogueSO[] _firstTextDatas;
        private Animator _animator;
        private bool _isSpeking;
        private DialoguePopupUI _dialogueUi;

        private void Awake()
        {
            _uiEventChannelSO = Managers.Resource.Load<GameEventChannelSO>("UIEventChannelSO");
            _animator = GetComponentInChildren<Animator>();
        }

        public void Interact(Transform Interactor)
        {
            if (_isSpeking == true)
                return;

            _isSpeking = true;
            _animator.SetTrigger("Interact");

            if (Managers.Game.IsTutorialCompleted() == true)
            {
                var dialogue = Managers.UI.ShowPopup<DialoguePopupUI>();
                dialogue.ShowText(_firstTextDatas[Random.Range(0, _firstTextDatas.Length)], isFinishMove: true);
                dialogue.DialogueFinishEvent += () => _isSpeking = false;

            }
            else
            {
                _dialogueUi = Managers.UI.ShowPopup<DialoguePopupUI>("DialoguePopupUI");
                _dialogueUi.ShowText(_currentTextDatas[Random.Range(0, _currentTextDatas.Length)]);
                _dialogueUi.DialogueFinishEvent += HandleEndDialogue;
            }

        }

        private void HandleEndDialogue()
        {
            UIEvent.ShopInteractEvent.isOpen = true;
            _uiEventChannelSO.RaiseEvent(UIEvent.ShopInteractEvent);

            _isSpeking = false;
        }

        private void OnDestroy()
        {
            if (_dialogueUi)
                _dialogueUi.DialogueFinishEvent -= HandleEndDialogue;
        }
    }
}