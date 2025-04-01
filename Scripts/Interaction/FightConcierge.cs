using UnityEngine;
using Main.Shared;
using BIS.Events;
using BIS.Data;
using BIS.Manager;
using Main.Runtime.Core.Events;
using BIS.UI.Popup;

namespace BIS.Interactions
{
    public class FightConcierge : MonoBehaviour, IInteractable
    {
        public Transform UIDisplayTrm => transform;

        public Vector3 AdditionalUIDisplayPos => Vector3.up * 1;

        [field: SerializeField] public string Description { get; set; }
        [SerializeField] private DialogueSO[] _dialogueSOs;
        [SerializeField] private DialogueSO[] _firstDialogueSOs;
        private GameEventChannelSO _uiEventChannelSO;
        private DialoguePopupUI _dialoguePopupUI;
        private void Awake()
        {
            _uiEventChannelSO = Managers.Resource.Load<GameEventChannelSO>("UIEventChannelSO");
        }

        public void Interact(Transform Interactor)
        {
            if (Managers.Game.IsTutorialCompleted() == false)
            {
                _dialoguePopupUI = Managers.UI.ShowPopup<DialoguePopupUI>();
                _dialoguePopupUI.ShowText(_dialogueSOs[Random.Range(0, _dialogueSOs.Length)]);
                _dialoguePopupUI.DialogueFinishEvent += () =>
                    {
                        UIEvent.EnemyPreviewUIEvent.isOpen = true;
                        _uiEventChannelSO.RaiseEvent(UIEvent.EnemyPreviewUIEvent);
                    };
            }
            else
            {
                Managers.UI.ShowPopup<DialoguePopupUI>().ShowText(_firstDialogueSOs[Random.Range(0, _firstDialogueSOs.Length)], isFinishMove: true);
            }

        }

    }
}