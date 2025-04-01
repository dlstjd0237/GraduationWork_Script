using BIS.Events;
using BIS.Shared;
using Main.Runtime.Core.Events;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using static BIS.Core.Utility.Util;
using UnityEngine.UI;
using System;
using BIS.Data;
using BIS.Manager;

namespace BIS.UI.Popup
{
    //Run when creating UI
    public class NewEnemyChoiceUI : UIBase
    {
        private enum Iamges
        {
            EnemyIcon_Image,
            EnemyIconShadow_Image
        }

        private enum Texts
        {
            EnemyName_Text
        }


        private readonly int _shaderID = Shader.PropertyToID("_HalftoneFade");
        private readonly Vector2 _choicePos = new Vector2(320, -590);


        private GameEventChannelSO _uiEvent;
        [SerializeField] private int _uiIdxl;

        private Material[] _mats = new Material[2];
        private EnemyPartySO _currentEnemyData;
        private Vector2 _defualtPos;
        private float _duration = 0.75f;
        private bool _isChoice = false;

        public override bool Init()
        {
            if (base.Init() == false)
                return false;
            _uiEvent = Managers.Resource.Load<GameEventChannelSO>("UIEventChannelSO");
            _currentEnemyData = Managers.Game.EnemyPartySOs[_uiIdxl];

            // ==== UI Binds ====
            BindImages(typeof(Iamges));
            BindTexts(typeof(Texts));
            // ==================

            // ==== UI Event Bind ====
            BindEvent(GetImage((int)Iamges.EnemyIcon_Image).gameObject, HandleClickEvent, EUIEvent.Click);
            _uiEvent.AddListener<EnemyPriviewChoiceEvent>(HandleChoiceEvent);
            // =======================

            _mats[0] = GetImage((int)Iamges.EnemyIconShadow_Image).GetComponent<Image>().material;
            _mats[1] = GetImage((int)Iamges.EnemyIcon_Image).GetComponent<Image>().material;

            _defualtPos = (transform as RectTransform).anchoredPosition;

            _mats[0].SetFloat(0, _shaderID);
            _mats[1].SetFloat(0, _shaderID);


            SetUpUI();
            Show();
            return true;
        }

        public void SetUpUI()
        {
            GetImage((int)Iamges.EnemyIcon_Image).sprite = _currentEnemyData.MainUnit.UnitIcon;
            GetImage((int)Iamges.EnemyIconShadow_Image).sprite = _currentEnemyData.MainUnit.UnitIcon;
            GetText((int)Texts.EnemyName_Text).text = _currentEnemyData.MainUnit.UnitDisplayName;
        }

        private void HandleChoiceEvent(EnemyPriviewChoiceEvent evt)
        {
            bool isChoiceUI = evt.enemyPartySO == _currentEnemyData;
            if (isChoiceUI) //선택 됬을 때
            {
                Choice();
            }
            else
            {
                Close();
            }
        }

        private void HandleClickEvent(PointerEventData evt)
        {
            if (_isChoice == true)
                return;

            UIEvent.EnemyPrevieChoiceEvent.enemyPartySO = _currentEnemyData;
            _uiEvent.RaiseEvent(UIEvent.EnemyPrevieChoiceEvent);

            _isChoice = true;
        }


        public void Choice()
        {
            (transform as RectTransform).DOAnchorPos(_choicePos, _duration);
        }

        public void Show()
        {
            _isChoice = false;
            UIFadeOut(gameObject, false, _duration);
            _mats[0].DOFloat(10, _shaderID, _duration);
            _mats[1].DOFloat(10, _shaderID, _duration);
            (transform as RectTransform).DOAnchorPos(_defualtPos, _duration);
        }

        public void Close()
        {
            _mats[0].DOFloat(0, _shaderID, _duration);
            _mats[1].DOFloat(0, _shaderID, _duration).OnComplete(() => UIFadeOut(gameObject, true, 0));
        }

        private void OnDestroy()
        {
            _mats[0].SetFloat(0, _shaderID);
            _mats[1].SetFloat(0, _shaderID);
        }

        private void OnDisable()
        {
            _uiEvent.RemoveListener<EnemyPriviewChoiceEvent>(HandleChoiceEvent);

            _mats[0].SetFloat(0, _shaderID);
            _mats[1].SetFloat(0, _shaderID);
        }
    }
}