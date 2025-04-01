using BIS.Data;
using BIS.Manager;
using BIS.UI.Popup;
using FIMSpace.Basics;
using Main.Runtime.Core.Events;
using Main.Shared;
using System;
using UnityEngine;

namespace BIS
{
    public class TutorialNPC : MonoBehaviour, IInteractable
    {
        public Transform UIDisplayTrm => transform;

        public Vector3 AdditionalUIDisplayPos => Vector3.up * 1;

        [field: SerializeField] public string Description { get; set; }
        [SerializeField] private DialogueSO _currentTextData;
        private bool _isSpeking;

        public void Interact(Transform Interactor)
        {
            if (_isSpeking == true)
                return;

            _isSpeking = true;

            DialoguePopupUI ui = Managers.UI.ShowPopup<DialoguePopupUI>("DialoguePopupUI");
            ui.ShowText(_currentTextData);
            ui.DialogueFinishEvent += HandleEndDialogue;
        }

        private void HandleEndDialogue()
        {
            SceneControlManager.LoadScene("KHJTutorialScene");
        }
    }
}
