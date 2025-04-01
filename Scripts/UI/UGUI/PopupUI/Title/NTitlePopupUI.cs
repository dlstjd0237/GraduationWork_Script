using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using System;
using BIS.Core.Utility;
using DG.Tweening;
using BIS.Shared;
using UnityEngine.EventSystems;
using BIS.Manager;
using BIS.Data;
using UnityEngine.Events;
using Unity.Cinemachine;
using UnityEngine.Rendering;

namespace BIS.UI.Popup
{
    public class NTitlePopupUI : PopupUI
    {
        private enum BtnType
        {
            GameStart,
            Story,
            Exit
        }

        private enum Objects
        {
            DefaultCamera,
            StoryCamera,
            ExitCamera
        }

        [SerializeField] private ConfirmationSO _gamePlayerConfirmationSO;
        [SerializeField] private ConfirmationSO _gameExitConfirmationSO;
        [SerializeField] private InformationSO _informationSO;
        [SerializeField] private UnityEvent _titleEnterEvent;
        [SerializeField] private VolumeProfile _volumeProfile;
        [SerializeField] private string _nextSceneName;
        private CinemachineCamera _startCam;
        private CinemachineCamera _storyCam;
        private CinemachineCamera _exitCam;
        private Dictionary<BtnType, (Image, TMP_Text)> _choiceDic;


        public override bool Init()
        {
            if (base.Init() == false)
                return false;

            Managers.UI.SetCanvas(gameObject, false, 0);

            BindObjects(typeof(Objects));

            _startCam = GetObject((int)Objects.DefaultCamera).GetComponent<CinemachineCamera>();
            _storyCam = GetObject((int)Objects.StoryCamera).GetComponent<CinemachineCamera>();
            _exitCam = GetObject((int)Objects.ExitCamera).GetComponent<CinemachineCamera>();


            if (_volumeProfile.TryGet<Beautify.Universal.Beautify>(out var bloom))
            {
                bloom.vignettingBlink.Override(0);
            }

            _choiceDic = new Dictionary<BtnType, (Image, TMP_Text)>();

            foreach (BtnType item in Enum.GetValues(typeof(BtnType)))
            {
                TMP_Text text = Util.FindChild<TMP_Text>(gameObject, $"{item.ToString()}_Text", true);
                Image image = Util.FindChild<Image>(gameObject, $"{item.ToString()}_Image", true);
                _choiceDic.Add(item, (image, text));
            }

            foreach (var item in _choiceDic)
            {
                BindEvent(item.Value.Item2.gameObject, (evt) =>
                    {
                        item.Value.Item2.DOColor(Color.black, 0.25f);
                        item.Value.Item1.DOFade(1, 0.25f);
                    }
                    , EUIEvent.PointerEnter);
                BindEvent(item.Value.Item2.gameObject, (evt) =>
                    {
                        item.Value.Item2.DOColor(Color.white, 0.25f);
                        item.Value.Item1.DOFade(0, 0.25f);
                    }
                    , EUIEvent.PointerExit);
            }

            BindEvent(_choiceDic[BtnType.GameStart].Item2.gameObject, HandlePlayGame, EUIEvent.Click);
            BindEvent(_choiceDic[BtnType.Story].Item2.gameObject, HandleStory, EUIEvent.Click);
            BindEvent(_choiceDic[BtnType.Exit].Item2.gameObject, HandleExitGame, EUIEvent.Click);

            return true;
        }

        private void HandleStory(PointerEventData evt)
        {
            Main.Runtime.Manager.Managers.FMODManager.PlayTextClickSound();
            _startCam.Priority = 0;
            _exitCam.Priority = 0;
            _storyCam.Priority = 100;
            var informationUI = Managers.UI.ShowPopup<InformationPopupUI>();
            informationUI.CloseEvent += HandleCloseEvent;
            informationUI.SetUpUI(_informationSO);
        }

        private void HandleCloseEvent()
        {
            Main.Runtime.Manager.Managers.FMODManager.PlayButtonClickSound();
            Managers.UI.ClosePopupUI();
            _exitCam.Priority = 0;
            _storyCam.Priority = 0;
            _startCam.Priority = 100;
        }

        private void HandleExitGame(PointerEventData evt)
        {
            Main.Runtime.Manager.Managers.FMODManager.PlayTextClickSound();
            _exitCam.Priority = 100;
            _storyCam.Priority = 0;
            _startCam.Priority = 0;
            var confirmationUI = Managers.UI.ShowPopup<ConfirmationPopupUI>();
            confirmationUI.SetUpUI(_gameExitConfirmationSO, (evt) =>
            {
                Main.Runtime.Manager.Managers.FMODManager.PlayButtonClickSound();

                ClosePopup();

                if (_volumeProfile.TryGet<Beautify.Universal.Beautify>(out var bloom))
                {
                    DOTween.To(() => bloom.vignettingBlink.value, x => bloom.vignettingBlink.Override(x), 1f, 2.5f)
                        .OnComplete(() => { Application.Quit(); });
                }
            }, (evt) =>
            {
                Main.Runtime.Manager.Managers.FMODManager.PlayButtonClickSound();
                _exitCam.Priority = 0;
                _storyCam.Priority = 0;
                _startCam.Priority = 100;
            });
        }

        private void HandlePlayGame(PointerEventData evt)
        {
            Main.Runtime.Manager.Managers.FMODManager.PlayTextClickSound();
            var confirmationUI = Managers.UI.ShowPopup<ConfirmationPopupUI>();
            confirmationUI.SetUpUI(_gamePlayerConfirmationSO, (evt) =>
                {
                    Main.Runtime.Manager.Managers.FMODManager.PlayButtonClickSound();
                    ClosePopup();

                    if (_volumeProfile.TryGet<Beautify.Universal.Beautify>(out var bloom))
                    {
                        Sequence seq = DOTween.Sequence();
                        seq.AppendCallback(() =>
                        {
                            GetObject((int)Objects.DefaultCamera).SetActive(false);
                            GetObject((int)Objects.ExitCamera).SetActive(false);
                            GetObject((int)Objects.StoryCamera).SetActive(false);
                            _titleEnterEvent?.Invoke();
                        });
                        seq.OnComplete(() => bloom.vignettingBlink.Override(0));
                    };
                }
                , (evt) =>
                {
                    Main.Runtime.Manager.Managers.FMODManager.PlayButtonClickSound();
                    SceneControlManager.LoadScene(_nextSceneName);
                });
        }
    }
}