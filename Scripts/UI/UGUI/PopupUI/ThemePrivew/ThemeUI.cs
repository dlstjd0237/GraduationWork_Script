using UnityEngine;
using UnityEngine.EventSystems;

using BIS.Shared;
using System;
using BIS.Data;
using Main.Runtime.Core.Events;
using BIS.Manager;
using BIS.Events;
using BIS.Core.Utility;
using DG.Tweening;

namespace BIS.UI.Popup
{
    public class ThemeUI : UIBase
    {
        private enum Images
        {
            ThemeIcon_Image,
            Frame_Image
        }
        private enum Texts
        {
            ThemeName_Text,
            ThemeInfo_Text
        }

        private const short _offset = 200;

        private RectTransform _themeIconRectTransform;

        private Vector2 _originalIconPosition;
        private Vector2 _originalIconScale;
        private Vector2 _originalFrameScale;
        private Vector2 _currentIconPosition;

        [SerializeField] private short _idx = 1;
        [SerializeField] private ThemeSO _themeSO;
        [SerializeField] private GameEventChannelSO _uiEventChannelSO;

        public override bool Init()
        {
            if (base.Init() == false)
                return false;

            //_uiEventChannelSO = Managers.Resource.Load<GameEventChannelSO>("UIEventChannelSO");
            _uiEventChannelSO.AddListener<ThemPriviewChoiceEvent>(HandleChoice);
            BindImages(typeof(Images));
            BindTexts(typeof(Texts));

            _themeIconRectTransform = GetImage((int)Images.ThemeIcon_Image).GetComponent<RectTransform>();

            _originalIconPosition = _themeIconRectTransform.localPosition;

            _originalFrameScale.x = GetImage((int)Images.Frame_Image).rectTransform.rect.width;
            _originalFrameScale.y = GetImage((int)Images.Frame_Image).rectTransform.rect.height;

            _originalIconScale.x = GetImage((int)Images.ThemeIcon_Image).rectTransform.rect.width;
            _originalIconScale.y = GetImage((int)Images.ThemeIcon_Image).rectTransform.rect.height;


            BindEvent(GetImage((int)Images.Frame_Image).gameObject, HandleImageMove, EUIEvent.PointerMove);
            BindEvent(GetImage((int)Images.Frame_Image).gameObject, HandleImageExit, EUIEvent.PointerExit);
            BindEvent(GetImage((int)Images.Frame_Image).gameObject, HandleClick, EUIEvent.Click);

            return true;
        }

        private void HandleChoice(ThemPriviewChoiceEvent evt)
        {
            if (evt.themeSO == _themeSO)
                return;

            Util.UIFadeOut(gameObject, true, 0.5f);
        }

        private void HandleClick(PointerEventData evt)
        {
            transform.DOLocalMove(Vector2.zero, 0.5f);
            UIEvent.ThemPriviewChoiceEvent.themeSO = _themeSO;
            _uiEventChannelSO.RaiseEvent(UIEvent.ThemPriviewChoiceEvent);
        }

        private void HandleImageExit(PointerEventData evt)
        {
            _themeIconRectTransform.localPosition = new Vector2(0, 0);
        }

        private void HandleImageMove(PointerEventData evt)
        {
            Vector2 mousePosition = evt.position;
            int x = _idx == 1 ? 50 : 650 + (50 * _idx);

            float normalizedX = Mathf.InverseLerp(x, _originalFrameScale.x * _idx, mousePosition.x);
            float normalizedY = Mathf.InverseLerp(100, _originalFrameScale.y + 100, mousePosition.y);

            Vector2 offset = new Vector2(
                Mathf.Lerp(-_offset, _offset, normalizedX),  // -50에서 +50까지 비례적으로 변동
                Mathf.Lerp(-_offset, _offset, normalizedY)   // -50에서 +50까지 비례적으로 변동
            );

            _currentIconPosition = Vector2.Lerp(_currentIconPosition, _originalIconPosition + (offset * -1), 0.2f);

            _themeIconRectTransform.localPosition = _currentIconPosition;
        }
    }
}
