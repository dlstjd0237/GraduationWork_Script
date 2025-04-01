using BIS.Core.Utility;
using BIS.Events;
using BIS.Manager;
using BIS.Shared.Interface;
using DG.Tweening;
using KHJ.SO;
using Main.Runtime.Core.Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BIS.UI
{
    public class ShopItemUI : UIBase, ISynergyChoiceable
    {
        private enum Texts
        {
            ItemNameText,
            PriceText
        }

        private enum Images
        {
            ItemImageIcon
        }

        //==== 상수화 ====
        private readonly Color ItemDefaultColor = new Color(0.2470588f, 0.2745098f, 0.3058824f);
        private readonly Color ItemChoiceColor = new Color(0.9254903f, 0.8980393f, 0.8470589f);
        private const float ChoiceScale = 1.05f;
        private const float DefaultScale = 1f;
        private const float AnimationDuration = 0.2f;
        //================

        private GameEventChannelSO _uiEventChannel;
#if UNITY_EDITOR
        [SerializeField] private SynergySO _testItemSO;
#endif
        private SynergySO _itemData;
        public SynergySO ItemData => _itemData;


        private Button _button;


        public override bool Init()
        {
            if (base.Init() == false)
                return false;
            _uiEventChannel = Managers.Resource.Load<GameEventChannelSO>("UIEventChannelSO");
            _button = GetComponent<Button>();
            _button.onClick.AddListener(HandleItemChoiceEvent);

#if UNITY_EDITOR
            //DataBinding(_testItemSO);
#endif

            return true;
        }

        public void DataBinding(SynergySO itemSO)
        {
            _itemData = itemSO;

            if (itemSO.ValueNullCheck() == false)
                return;

            gameObject.name = $"[{itemSO.ItemName}]Item";

            //==== UI Bind====
            BindTexts(typeof(Texts));
            BindImages(typeof(Images));
            //================


            GetText((int)Texts.ItemNameText).SetText(_itemData.ItemName);
            GetText((int)Texts.PriceText).SetText(_itemData.ItemPrice.ToString());
            GetImage((int)Images.ItemImageIcon).sprite = _itemData.ItemIcon;
        }

        private void HandleItemChoiceEvent()
        {
            ShopItemChoice evt = new ShopItemChoice();
            evt.shopItemUI = this;
            _uiEventChannel.RaiseEvent(evt);
        }

        public void SetChoiceState(bool isChoice)
        {
            Image image = GetComponent<Image>();
            TMP_Text text = GetText((int)Texts.ItemNameText);

            _button.transform.DOScale(isChoice ? ChoiceScale : DefaultScale, AnimationDuration);
            Util.UIColorChange<TMP_Text>(text.gameObject, isChoice ? Color.black : Color.white);
            Util.UIColorChange<Image>(image.gameObject, isChoice ? ItemChoiceColor : ItemDefaultColor);
        }


        public void PurchaseItem()
        {
            //이곳에 구매 로직 넣어야함
            Managers.Resource.Destroy(gameObject);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(HandleItemChoiceEvent);
        }
    }
}