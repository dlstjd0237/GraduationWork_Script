//using UnityEngine;
//using BIS.Data;
//using UnityEngine.EventSystems;
//using System;
//using BIS.Shared;
//using BIS.Manager;
//using BIS.Core.Utility;
//using System.Collections.Generic;
//using Main.Runtime.Core.Events;
//using BIS.Events;
//using BIS.Shared.Interface;
//using Random = UnityEngine.Random;
//using UnityEngine.UI;

//namespace BIS.UI.Popup
//{
//    public class EnemyPrivewPopupUI : PopupUI
//    {
//        private readonly Color _defualtButtonColor = new Color(0.7137255f, 0.7176471f, 0.7803922f);
//        private enum Buttons
//        {
//            BattleStart_Button,
//            ExitButton
//        }

//        private enum Texts
//        {
//            TotalRanke_Text,
//            EnemtInfo_Text
//        }

//        [SerializeField] private GameEventChannelSO _gameEventChannel;
//        [SerializeField] private List<EnemyPartySO> _nextStageData;
//        private Transform _enemySpawnInfoRoot;
//        private IUnitChoiceable _choiceUI;
//        private List<EnemyPrivewDescriptionUI> _enemyPrivewDescriptions;
//        private Stack<string> _unitSONames;
//        public override bool Init()
//        {
//            if (base.Init() == false)
//                return false;


//            BindButtons(typeof(Buttons));
//            BindTexts(typeof(Texts));

//            _enemySpawnInfoRoot = Util.FindChild<Transform>(gameObject, "SpawnEnemyRoot", true);
//            _gameEventChannel.AddListener<UnitChoiceUIEvent>(HandleUnitChoiceEvent);

//            EnemyPrivewSetUp(_nextStageData);
//            ExitButtonSetting();
//            return true;
//        }

//        private void EnemyPrivewSetUp(List<EnemyPartySO> data)
//        {
//            _enemyPrivewDescriptions = new List<EnemyPrivewDescriptionUI>();
//            _unitSONames = new Stack<string>();

//            BindEvent(GetButton((int)Buttons.BattleStart_Button).gameObject, HandleStartButton, EUIEvent.Click);
//            int totalRank = 0;
//            for (int i = 0; i < data.Count; ++i)
//            {
//                for (int j = 0; j < data[i].UnitDatas.Count; ++j)
//                {
//                    UnitSO unitSO = data[i].MainUnit;

//                    EnemyPrivewDescriptionUI enemyPrivewUI = Managers.UI.MakeSupItem<EnemyPrivewDescriptionUI>(_enemySpawnInfoRoot, "EnemyPrivewDescriptionUI");
//                    int rank = Random.Range(1, 5000);
//                    totalRank += rank;
//                    enemyPrivewUI.SetUpUI(unitSO, rank);
//                    _enemyPrivewDescriptions.Add(enemyPrivewUI);
//                    _unitSONames.Push(unitSO.UnitDisplayName);
//                }
//            }

//            GetText((int)Texts.TotalRanke_Text).text = $"평균 순위 : {totalRank/3} 위";

//             _choiceUI = _enemyPrivewDescriptions[0];
//            _choiceUI.SetChoiceState(true);
//        }

//        private void Description(UnitSO data)
//        {
//            GetText((int)Texts.EnemtInfo_Text).text = data.UnitDescription;
//        }

//        private void HandleUnitChoiceEvent(UnitChoiceUIEvent evt)
//        {
//            _choiceUI.SetChoiceState(false);
//            _choiceUI = evt.isClick;
//            _choiceUI.SetChoiceState(true);
//            Description(evt.isClick.Data);
//        }

//        #region Exit
//        private void ExitButtonSetting()
//        {
//            Button btn = GetButton((int)Buttons.ExitButton);

//            btn.onClick.AddListener(HandleExitClickEvent);
//            BindEvent(btn.gameObject, HandleExitPointerEnter, EUIEvent.PointerEnter);
//            BindEvent(btn.gameObject, HandleEnterPointerExit, EUIEvent.PointerExit);
//        }

//        private void HandleExitClickEvent()
//        {
//            var evt = UIEvent.EnemyPreviewUIEvent;
//            evt.isOpen = false;
//            _gameEventChannel.RaiseEvent(evt);
//        }

//        private void HandleEnterPointerExit(PointerEventData obj)
//        {
//            GameObject go = GetButton((int)Buttons.ExitButton).gameObject;
//            Util.UIColorChange<Image>(go, _defualtButtonColor);
//        }
//        private void HandleExitPointerEnter(PointerEventData obj)
//        {
//            GameObject go = GetButton((int)Buttons.ExitButton).gameObject;
//            Util.UIColorChange<Image>(go, Color.white);
//        }
//        #endregion

//        private void OnDestroy()
//        {
//            _gameEventChannel.RemoveListener<UnitChoiceUIEvent>(HandleUnitChoiceEvent);
//        }


//        private void HandleStartButton(PointerEventData obj)
//        {
//            //만약 씬전환하는거 확정나면 그걸로 변경
//            SceneControlManager.LoadScene("BattleScene");
//        }
//    }
//}
