using BIS.Data;
using BIS.Shared;
using BIS.Core.Utility;
using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using PJH.Players;
using BIS.Manager;

namespace BIS.UI.Popup
{
    public class ConfirmationPopupUI : PopupUI
    {
        private enum Texts
        {
            Description_Text,
            Yes_Text,
            No_Text
        }

        [SerializeField] private PlayerInputSO _playerInputSO;

        private readonly Color _defualtColor = new Color(0.9f, 0.9f, 0.9f);

        private RectTransform _confirmationRoot;

        public void SetUpUI(ConfirmationSO data, Action<PointerEventData> yesBtnEvent, Action<PointerEventData> noBtnEvent = null)
        {
            _playerInputSO.EnableUIInput(false);

            _confirmationRoot = Util.FindChild<RectTransform>(gameObject, "Background_Image", true);
            _confirmationRoot.DOAnchorPosY(0, 0.4f).SetEase(Ease.OutQuad);


            BindTexts(typeof(Texts));

            GetText((int)Texts.Description_Text).text = data.Description;

            BindEvent((GetText((int)Texts.Yes_Text).gameObject), yesBtnEvent, EUIEvent.Click);
            BindEvent((GetText((int)Texts.Yes_Text).gameObject), (evt) => Managers.UI.ClosePopupUI(this), EUIEvent.Click);

            BindEvent((GetText((int)Texts.No_Text).gameObject), noBtnEvent, EUIEvent.Click);
            BindEvent((GetText((int)Texts.No_Text).gameObject), (evt) => Managers.UI.ClosePopupUI(this), EUIEvent.Click);

            BindEvent((GetText((int)Texts.Yes_Text).gameObject), delegate { HandleBtnHoverEvent(GetText((int)Texts.Yes_Text), Color.white); }, EUIEvent.PointerEnter);
            BindEvent((GetText((int)Texts.No_Text).gameObject), delegate { HandleBtnHoverEvent(GetText((int)Texts.No_Text), Color.white); }, EUIEvent.PointerEnter);

            BindEvent((GetText((int)Texts.Yes_Text).gameObject), delegate { HandleBtnHoverEvent(GetText((int)Texts.Yes_Text), _defualtColor); }, EUIEvent.PointerExit);
            BindEvent((GetText((int)Texts.No_Text).gameObject), delegate { HandleBtnHoverEvent(GetText((int)Texts.No_Text), _defualtColor); }, EUIEvent.PointerExit);
        }

        public override void ClosePopup(Action callBack = null)
        {
            _playerInputSO.EnableUIInput(true);
            _confirmationRoot.DOAnchorPosY(-700, 0.7f).SetEase(Ease.OutQuad).OnComplete(delegate { Util.UIFadeOut(gameObject, true, 0.4f, callBack); });
        }

        private void HandleBtnHoverEvent(TMP_Text text, Color color)
        {
            text.DOColor(color, 0.3f);
        }
    }
}
