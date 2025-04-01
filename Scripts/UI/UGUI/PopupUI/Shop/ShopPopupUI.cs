using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Main.Runtime.Core.Events;
using BIS.Shared;
using BIS.Core.Utility;
using BIS.Data;
using BIS.Shared.Interface;
using BIS.Manager;
using BIS.Events;
using PJH.Players;
using KHJ.Enum;
using KHJ.SO;
using DG.Tweening;
using KHJ;
using System.Collections;
using LJS.Item;

namespace BIS.UI.Popup
{
    public class ShopPopupUI : PopupUI, ISavable
    {
        private enum Buttons
        {
            PurchaseButton,
            ExitButton
        }

        private enum Texts
        {
            Item_Name_Text,
            Item_Stat_Amount_Text,
            Item_Stat_Text,
            Description_Text,
            GoodsText,
            Synergy_Text
        }

        private enum Images
        {
            Item_Image,
            Item_Stat_Icon,
            GoodsIcon,
            Synergy_Icon_Image
        }

        private enum GameObjects
        {
            DescriptionRoot
        }

        private readonly Color _defualtButtonColor = new Color(0.7137255f, 0.7176471f, 0.7803922f);

        private GameEventChannelSO _uiEventChannelSO;
        [SerializeField] private SynergyIconListSO _synergyIconListSO;
        [SerializeField] private StatIconListSO _statIconlistSO;
        [SerializeField] private PlayerInputSO _playerInput;
        [SerializeField] private CurrencySO _moneySO;
        [field: SerializeField] public SaveIDSO IdData { get; set; }

        private SynergyBlockTableSO _items;
        private int _price;
        private List<ISynergyChoiceable> _shopItems;
        private Transform _root;
        private ISynergyChoiceable _shopItemUI;
        private Dictionary<SynergyType, Sprite> _synergyIconDictionary;
        private Dictionary<string, Sprite> _statIconDictionary;


        public override bool Init()
        {
            if (base.Init() == false)
                return false;

            _uiEventChannelSO = Managers.Resource.Load<GameEventChannelSO>("UIEventChannelSO");
            _items = Managers.Resource.Load<SynergyBlockTableSO>("SynergyBlockTableSO");
            _playerInput.ESCEvent += HandleExitClickEvent;


            //==== UI Binding ====
            BindButtons(typeof(Buttons));
            BindImages(typeof(Images));
            BindTexts(typeof(Texts));
            BindObjects(typeof(GameObjects));

            PurchaseButtonSetting();
            ExitButtonSetting();
            //=================



            //==== Root Setting ====
            _root = Util.FindChild<Transform>(gameObject, "Content", true);
            if (_root.ValueNullCheck() == false)
                return false;

            RectTransform rootTrm = (_root.transform as RectTransform);
            float rootHeight = (5 * 150) + ((5 - 1) * 40);
            rootTrm.sizeDelta = new Vector2(rootTrm.sizeDelta.x, rootHeight);
            //======================


            // ==== Goods Setting ====
            GetImage((int)Images.GoodsIcon).sprite = _moneySO.CurrencyIcon;
            HandleMoneyChangeEvent(_moneySO.CurrentAmmount);
            _moneySO.ValueChangeEvent += HandleMoneyChangeEvent;
            // =======================

            // ==== Synergy Icon Dictionary Init ====
            _synergyIconDictionary = new Dictionary<SynergyType, Sprite>();
            List<SynergyIconData> iconList = _synergyIconListSO.SynergyIconDatas;
            for (int i = 0; i < iconList.Count; ++i)
                _synergyIconDictionary.Add(iconList[i].synergyType, iconList[i].icon);
            // ======================================


            // ==== Stat Icon dictionar Init ====
            _statIconDictionary = new Dictionary<string, Sprite>();
            List<StatIconData> statList = _statIconlistSO.StatIconList;
            for (int i = 0; i < statList.Count; ++i)
                _statIconDictionary.Add(statList[i].statSO.DisplayName, statList[i].icon);
            // ==================================


            // ==== Stat Icon Event ====
            BindEvent(GetImage((int)Images.Item_Stat_Icon).gameObject, IconEventPointEnter, EUIEvent.PointerEnter);
            BindEvent(GetImage((int)Images.Item_Stat_Icon).gameObject, IconEventPointExit, EUIEvent.PointerExit);
            // =========================

            _shopItems = new List<ISynergyChoiceable>();

            for (int i = 0; i < 5; ++i)
            {
                //GameObject go = Managers.Resource.Instantiate("ShopItemUI", _root, true);
                ShopItemUI go = Managers.UI.MakeSupItem<ShopItemUI>(_root, "ShopItemUI", true);
                ISynergyChoiceable ui = go.GetComponent<ISynergyChoiceable>();

                SynergySO so = _items.synergyList[Random.Range(0, _items.synergyList.Count)];
                ui.DataBinding(so);
                if (i == 0)
                {
                    SetUpDescription(so);
                    _shopItemUI = ui;
                    _shopItemUI.SetChoiceState(true);
                }

                _shopItems.Add(ui);
            }

            _uiEventChannelSO.AddListener<ShopItemChoice>(HandleItemChoiceEvent);

            return true;
        }

        private void IconEventPointExit(PointerEventData evt)
        {
            GetText((int)Texts.Item_Stat_Text).DOFade(0, 0.1f);
        }

        private void IconEventPointEnter(PointerEventData evt)
        {
            GetText((int)Texts.Item_Stat_Text).DOFade(1, 0.1f);
        }

        private void HandleMoneyChangeEvent(int newValue) =>
            GetText((int)Texts.GoodsText).text = newValue.ToString("#,0");

        private void HandleItemChoiceEvent(ShopItemChoice evt)
        {
            if (_shopItemUI != null)
                _shopItemUI.SetChoiceState(false);


            SetUpDescription(evt.shopItemUI.ItemData);
            _shopItemUI = evt.shopItemUI;
            _shopItemUI.SetChoiceState(true);
        }

        private void SetUpDescription(SynergySO choiceItem)
        {
            if (choiceItem.ValueNullCheck() == false)
                return;

            GetImage((int)Images.Item_Image).sprite = choiceItem.ItemIcon;
            GetText((int)Texts.Item_Name_Text).text = choiceItem.ItemName;
            GetImage((int)Images.Synergy_Icon_Image).sprite = _synergyIconDictionary[choiceItem.synergyType];
            GetImage((int)Images.Item_Stat_Icon).sprite = _statIconDictionary[choiceItem.increasePlayerStat.DisplayName];
            GetText((int)Texts.Synergy_Text).text = $"[{choiceItem.synergyType.ToString()}]";
            GetText((int)Texts.Description_Text).text = choiceItem.ItemDescription;
            GetText((int)Texts.Item_Stat_Text).text = choiceItem.increasePlayerStat.DisplayName;
            GetText((int)Texts.Item_Stat_Amount_Text).text = choiceItem.increasePlayerStatValue.ToString();
            _price = choiceItem.ItemPrice;
        }


        #region ExitButton

        private void ExitButtonSetting()
        {
            Button btn = GetButton((int)Buttons.ExitButton);

            btn.onClick.AddListener(HandleExitClickEvent);
            BindEvent(btn.gameObject, HandleExitPointerEnter, EUIEvent.PointerEnter);
            BindEvent(btn.gameObject, HandleEnterPointerExit, EUIEvent.PointerExit);
        }

        private void HandleExitClickEvent()
        {
            BIS.Events.UIEvent.ShopInteractEvent.isOpen = false;
            _uiEventChannelSO.RaiseEvent(BIS.Events.UIEvent.ShopInteractEvent);
        }

        private void HandleEnterPointerExit(PointerEventData obj)
        {
            GameObject go = GetButton((int)Buttons.ExitButton).gameObject;
            Util.UIColorChange<Image>(go, _defualtButtonColor);
        }

        private void HandleExitPointerEnter(PointerEventData obj)
        {
            GameObject go = GetButton((int)Buttons.ExitButton).gameObject;
            Util.UIColorChange<Image>(go, Color.white);
        }
        #endregion

        #region PurchaseButton

        private void PurchaseButtonSetting()
        {
            Button btn = GetButton((int)Buttons.PurchaseButton);
            btn.onClick.AddListener(HandlePurchaseClickEvent);
            BindEvent(btn.gameObject, HandlePurchasePointerEnter, EUIEvent.PointerEnter);
            BindEvent(btn.gameObject, HandlePurchasePointerExit, EUIEvent.PointerExit);
        }

        private void HandlePurchaseClickEvent()
        {
            if (_shopItems.Count <= 0)
                return;

            //�̰����� ���� ��
            var purchaseData = _moneySO.Purchase(_price);
            if (purchaseData.isPurchasable == false)
                return;
            if (SynergyBoardManager.Instance.SetFistSlotBlock(_shopItemUI.ItemData, true) == false)
                return;

            _shopItemUI.PurchaseItem();
            _shopItems.Remove(_shopItemUI);

            if (_shopItems.Count <= 0)
            {
                Util.UIFadeOut(GetObject((int)GameObjects.DescriptionRoot), true);
                return;
            }

            _shopItemUI = _shopItems[0];
            _shopItemUI.SetChoiceState(true);
            SetUpDescription(_shopItemUI.ItemData);
        }

        private void HandlePurchasePointerExit(PointerEventData obj)
        {
            GameObject go = GetButton((int)Buttons.PurchaseButton).gameObject;
            Util.UIColorChange<Image>(go, _defualtButtonColor);
        }

        private void HandlePurchasePointerEnter(PointerEventData obj)
        {
            GameObject go = GetButton((int)Buttons.PurchaseButton).gameObject;
            Util.UIColorChange<Image>(go, Color.white);
        }

        #endregion


        public string GetSaveData()
        {
            return "??";
        }

        public void RestoreData(string data)
        {
            Debug.Log(data);
            Debug.Log(JsonUtility.ToJson(data));
        }

        private void OnDestroy()
        {
            _playerInput.ESCEvent -= HandleExitClickEvent;
            _moneySO.ValueChangeEvent -= HandleMoneyChangeEvent;
            _uiEventChannelSO.RemoveListener<ShopItemChoice>(HandleItemChoiceEvent);
        }

    }
}