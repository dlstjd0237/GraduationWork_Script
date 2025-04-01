using BIS.Core.Utility;
using BIS.Data;
using BIS.Events;
using BIS.Manager;
using BIS.Shared;
using Main.Runtime.Core.Events;
using PJH.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BIS.UI.Popup
{
    public class EnemyPrivewPopupUI : PopupUI
    {
        private readonly Color _defualtButtonColor = new Color(0.7137255f, 0.7176471f, 0.7803922f);

        [SerializeField] private PlayerInputSO _playerSO;
        private GameEventChannelSO _uiEventChannel;
        private Transform _descriptionCanvasTrm;
        private List<NewEnemyChoiceUI> newEnemyChoiceUIs;

        private enum Buttons
        {
            ExitButton,
            BattleStart_Button,
            ChoiceCanel_Button
        }

        private enum Texts
        {
            MainSpawnEnemyDescrption_Text,
            EnemyInfo_Text,
            PlayerRank_Text
        }

        public override bool Init()
        {
            if (base.Init() == false)
                return false;
            _uiEventChannel = Managers.Resource.Load<GameEventChannelSO>("UIEventChannelSO");
            _playerSO.ESCEvent += HandleExitClickEvent;
            // ==== UI Binding ====
            BindButtons(typeof(Buttons));
            BindTexts(typeof(Texts));
            // ====================

            // ==== Event Bind ====
            _uiEventChannel.AddListener<EnemyPriviewChoiceEvent>(HandleEnemyPrevieChoice);

            ExitButtonEventBind();
            BattleStartEventBind();
            CanelEventBind();
            // ====================

            newEnemyChoiceUIs = new List<NewEnemyChoiceUI>();
            newEnemyChoiceUIs = GetComponentsInChildren<NewEnemyChoiceUI>().ToList();

            _descriptionCanvasTrm = Util.FindChild<Transform>(gameObject, "StatPanel");
            GetText((int)Texts.PlayerRank_Text).text = Managers.Rank.GetPlayerRank();

            return true;
        }


        private void HandleEnemyPrevieChoice(EnemyPriviewChoiceEvent evt)
        {
            Util.UIFadeOut(_descriptionCanvasTrm.gameObject, false);
            DescriptionSetUp(evt.enemyPartySO);
        }

        private void DescriptionSetUp(EnemyPartySO data)
        {
            // ==== Main Enemy Description Setting ====
            string mainEnemyName = data.MainUnit.UnitDisplayName;
            int mainEnemyRank = data.MainUnit.Rank;
            string mainEnemyDescription = data.MainUnit.UnitDescription;

            string mainEnemyInfo =
                $"적 이름 : {mainEnemyName}\n" +
                $"적 랭킹 : {mainEnemyRank}위\n" +
                $"\n" +
                $"{mainEnemyDescription}";
            GetText((int)Texts.MainSpawnEnemyDescrption_Text).text = mainEnemyInfo;
            // ========================================


            // ==== Enemy Description Setting ====
            int enemyRankAve = data.GetEnemyAveRanking();
            int enemySpawnWave = data.UnitDatas.Count;

            string enemyInfo =
                $"적 평균 랭킹 : {enemyRankAve}위\n" +
                $"적 웨이브 : {enemySpawnWave}\n";

            GetText((int)Texts.EnemyInfo_Text).text = enemyInfo;
            // ===================================
        }

        #region Canel

        private void CanelEventBind()
        {
            Button btn = GetButton((int)Buttons.ChoiceCanel_Button);
            btn.onClick.AddListener(HandleCanelEvent);
            BindEvent(btn.gameObject,
                evt => { HandleExitPointerEnter(evt, GetButton((int)Buttons.ChoiceCanel_Button).gameObject); },
                EUIEvent.PointerEnter);
            BindEvent(btn.gameObject,
                evt => { HandleEnterPointerExit(evt, GetButton((int)Buttons.ChoiceCanel_Button).gameObject); },
                EUIEvent.PointerExit);
        }

        private void HandleCanelEvent()
        {
            for (int i = 0; i < newEnemyChoiceUIs.Count; ++i)
            {
                newEnemyChoiceUIs[i].Show();
            }

            Util.UIFadeOut(_descriptionCanvasTrm.gameObject, true);
            UIEvent.EnemyPrevieChoiceEvent.enemyPartySO = null;
        }

        #endregion


        #region BattleStart

        private void BattleStartEventBind()
        {
            Button btn = GetButton((int)Buttons.BattleStart_Button);
            btn.onClick.AddListener(HandleBattleStartEvent);
            BindEvent(btn.gameObject,
                evt => { HandleExitPointerEnter(evt, GetButton((int)Buttons.BattleStart_Button).gameObject); },
                EUIEvent.PointerEnter);
            BindEvent(btn.gameObject,
                evt => { HandleEnterPointerExit(evt, GetButton((int)Buttons.BattleStart_Button).gameObject); },
                EUIEvent.PointerExit);
        }

        private void HandleBattleStartEvent()
        {
            Managers.UI.ClosePopupUI(this);
            SceneControlManager.LoadScene("BattleScene");
        }

        #endregion

        #region Exit

        private void ExitButtonEventBind()
        {
            Button btn = GetButton((int)Buttons.ExitButton);

            btn.onClick.AddListener(HandleExitClickEvent);
            BindEvent(btn.gameObject,
                evt => { HandleExitPointerEnter(evt, GetButton((int)Buttons.ExitButton).gameObject); },
                EUIEvent.PointerEnter);
            BindEvent(btn.gameObject,
                evt => { HandleEnterPointerExit(evt, GetButton((int)Buttons.ExitButton).gameObject); },
                EUIEvent.PointerExit);
        }

        private void HandleExitClickEvent()
        {
            UIEvent.EnemyPreviewUIEvent.isOpen = false;
            _uiEventChannel.RaiseEvent(UIEvent.EnemyPreviewUIEvent);
        }

        #endregion


        private void HandleEnterPointerExit(PointerEventData obj, GameObject go)
        {
            Util.UIColorChange<Image>(go, _defualtButtonColor);
        }

        private void HandleExitPointerEnter(PointerEventData obj, GameObject go)
        {
            Util.UIColorChange<Image>(go, Color.white);
        }

        public override void ClosePopup(Action callBack = null)
        {
            for (int i = 0; i < newEnemyChoiceUIs.Count; ++i)
            {
                newEnemyChoiceUIs[i].Close();
            }

            base.ClosePopup(callBack);
        }

        private void OnDisable()
        {
            _playerSO.ESCEvent -= HandleExitClickEvent;
            _uiEventChannel.RemoveListener<EnemyPriviewChoiceEvent>(HandleEnemyPrevieChoice);
        }
    }
}