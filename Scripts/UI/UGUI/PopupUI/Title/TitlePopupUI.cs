using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Main.Runtime.Core.Events;
using PJH.Players;
using BIS.Manager;
using BIS.Events;
using BIS.Data;
using System;
using System.Collections;
using UnityEngine.Events;
using BIS.Core.Utility;

namespace BIS.UI.Popup
{
    public class TitlePopupUI : PopupUI
    {
        [SerializeField] private PlayerInputSO _inputSO;
        [SerializeField] private string _nextSceneName;
        [SerializeField] private UnityEvent _titleEnterEvent;
        [SerializeField] private bool _loadingComplete = false;
        [SerializeField] private ConfirmationSO _confirmationSO;
        private GameEventChannelSO _uiEvent;

        private enum Images
        {
            Title_Background
        }

        private enum Objects
        {
            TitleCamera
        }

        public override bool Init()
        {
            Managers.UI.SetCanvas(gameObject, false, 0);

            // ==== UI Bunding ====
            BindImages(typeof(Images));
            BindObjects(typeof(Objects));
            // ====================


            // ==== Title Icon Color =====
            Color targetColor = GetImage((int)Images.Title_Background).color;
            targetColor.a = 1;
            GetImage((int)Images.Title_Background).DOColor(targetColor, 10)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.Linear);
            // ===========================


            // ==== Input Action Binding ====
            _inputSO.ESCEvent += HandleShutDownEvent;
            _inputSO.EnterEvent += HandleGamePlayEvent;
            // ==============================
            StartCoroutine(TryGetUIEventChannel());

            return true;
        }

        IEnumerator TryGetUIEventChannel()
        {
            yield return new WaitUntil(() => Managers.Resource.IsLoaded);
            _uiEvent = Managers.Resource.Load<GameEventChannelSO>("UIEventChannelSO");

            _uiEvent.AddListener<LoadingUIEvent>(HandleLoadingUIEvent);
        }

        private void HandleLoadingUIEvent(LoadingUIEvent evt)
        {
            _loadingComplete = evt.isComplete;
        }

        private void HandleGamePlayEvent()
        {
            if (_loadingComplete == false) return;


            var confirmationUI = Managers.Resource.Instantiate("ConfirmationPopupUI")
                .GetComponent<ConfirmationPopupUI>();
            confirmationUI.SetUpUI(_confirmationSO, (evt) =>
                {
                    ClosePopup();
                    _titleEnterEvent?.Invoke();
                    GetObject((int)Objects.TitleCamera).SetActive(false);
                }
                , (evt) => SceneControlManager.LoadScene(_nextSceneName));
        }

        public override void ClosePopup(Action callBack = null)
        {
            Util.UIFadeOut(gameObject, true, 0.7f, callBack);
        }

        private void HandleShutDownEvent()
        {
            if (_loadingComplete == false) return;

            SceneControlManager.FadeOut(() => Application.Quit());
        }

        private void OnDestroy()
        {
            _uiEvent.RemoveListener<LoadingUIEvent>(HandleLoadingUIEvent);

            // ==== Input Action UnBinding ====
            _inputSO.ESCEvent -= HandleShutDownEvent;
            _inputSO.EnterEvent -= HandleGamePlayEvent;
            // ==============================
        }
    }
}