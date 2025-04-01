using UnityEngine;
using Main.Runtime.Core.Events;
using PJH.Players;
using BIS.Events;
using BIS.Manager;
using BIS.UI.Popup;
using BIS.Data;
using System.Collections.Generic;
using System.Linq;

namespace BIS.UI.Scenes
{
    public class GameSceneUI : SceneUI
    {
        [SerializeField] private PlayerInputSO _inputSO;
        [SerializeField] private EnemyPartyTableSO _enemyPartyTableSO;
        [SerializeField] private DialogueSO _dialogueSO;
        [SerializeField] private CurrencySO _mony;
        private GameEventChannelSO _uiEventChannelSO;

        private void Awake()
        {
            _uiEventChannelSO = Managers.Resource.Load<GameEventChannelSO>("UIEventChannelSO");
            _uiEventChannelSO.AddListener<ShopInteractEvent>(HandleShopOpenEvent);
            _uiEventChannelSO.AddListener<OptionEvent>(HandleOptionOpenEvent);
            _uiEventChannelSO.AddListener<EnemyPriviewUIEvent>(HandleEnemyPreviewUIEvent);

            _inputSO.ESCEvent += HandleOptionEvent;
            _inputSO.TabbarEvent += HandleinventoryEvent;


            // ==== Enemy Setting ====
            List<EnemyPartySO> enemyPartySOs = new List<EnemyPartySO>();
            for (int i = 0; i < 3; ++i)
            {
                EnemyPartySO enemySO =
                    _enemyPartyTableSO.EnemypartySOTable[Random.Range(0, _enemyPartyTableSO.EnemypartySOTable.Count)];

                enemySO.MainUnit.Rank = Managers.Rank.GetEnemyRandomRank();
                for (int j = 0; j < enemySO.UnitDatas.Count; ++j)
                {
                    for (int k = 0; k < enemySO.UnitDatas[j].spawnData.Count; ++k)
                    {
                        enemySO.UnitDatas[j].spawnData[k].spawnUnit.Rank = Managers.Rank.GetEnemyRandomRank();
                    }
                }

                enemyPartySOs.Add(enemySO);
            }

            Managers.Game.EnemyPartySOs = enemyPartySOs.ToList();
            // =======================

            Debug.Log(Managers.UI.GetPopupCount());
        }

        private void Start()
        {

            if (Managers.Game.IsTutorialCompleted() == true)
                Managers.UI.ShowPopup<DialoguePopupUI>().ShowText(_dialogueSO, isDontMove: false);
        }
        private void HandleinventoryEvent()
        {
            if (Managers.UI.GetPopupCount() >= 1 && UIEvent.InventoryEvent.isOpen == false)
                return;
            UIEvent.InventoryEvent.isOpen = !UIEvent.InventoryEvent.isOpen;
            _uiEventChannelSO.RaiseEvent(UIEvent.InventoryEvent);
        }

        private void HandleOptionEvent()
        {
            if (Managers.UI.GetPopupCount() >= 1 && UIEvent.OptionEvent.isOpen == false)
                return;
            UIEvent.OptionEvent.isOpen = !UIEvent.OptionEvent.isOpen;
            _uiEventChannelSO.RaiseEvent(UIEvent.OptionEvent);
        }

        private void HandleEnemyPreviewUIEvent(EnemyPriviewUIEvent evt)
        {
            if (evt.isOpen)
            {
                _inputSO.EnablePlayerInput(false);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Managers.UI.ShowPopup<EnemyPrivewPopupUI>(); //여기 클래스도 바꿔줘야함
            }
            else
            {
                _inputSO.EnablePlayerInput(true);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Managers.UI.ClosePopupUI();
            }
        }

        private void HandleOptionOpenEvent(OptionEvent evt)
        {


            if (evt.isOpen)
            {
                Debug.Log("ddd");
                _inputSO.EnablePlayerInput(false);
                //Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Managers.UI.ShowPopup<OptionPopupUI>();
            }
            else
            {
                _inputSO.EnablePlayerInput(true);
                //Time.timeScale = 1;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Managers.UI.CloseAllPopupUI();
            }
        }

        private void HandleShopOpenEvent(ShopInteractEvent evt)
        {
            if (evt.isOpen)
            {
                _inputSO.EnablePlayerInput(false);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Managers.UI.ShowPopup<ShopPopupUI>();
            }
            else
            {
                _inputSO.EnablePlayerInput(true);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Managers.UI.ClosePopupUI();
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                _mony.AddAmmount(1000);
            }

            if (Input.GetKeyDown(KeyCode.O))
                Managers.Game.IsTutorialComplete = true;
        }

        private void OnDestroy()
        {
            _uiEventChannelSO.RemoveListener<ShopInteractEvent>(HandleShopOpenEvent);
            _uiEventChannelSO.RemoveListener<OptionEvent>(HandleOptionOpenEvent);
            _uiEventChannelSO.RemoveListener<EnemyPriviewUIEvent>(HandleEnemyPreviewUIEvent);
            _inputSO.ESCEvent -= HandleOptionEvent;
            _inputSO.TabbarEvent -= HandleinventoryEvent;
        }
    }
}