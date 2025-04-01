using BIS.Data;
using BIS.Events;
using BIS.Shared.Interface;
using Main.Runtime.Core.Events;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

namespace BIS.UI.Popup
{
    public class EnemyPrivewDescriptionUI : UIBase, IUnitChoiceable
    {
        private enum Images
        {
            EnemyDescrpition_Image
        }

        private enum Texts
        {
            Name_Text,
            Rank_Text
        }
        [SerializeField] private GameEventChannelSO _uiEvenetChannelSO;
        private UnitSO _currentUnitSO;
        public UnitSO Data => _currentUnitSO;

        private Image _background;

        public void SetUpUI(UnitSO data, int rank)
        {
            _currentUnitSO = data;
            _background = GetComponent<Image>();
            BindImages(typeof(Images));
            BindTexts(typeof(Texts));

            GetText((int)Texts.Name_Text).text = $"이름 : {data.UnitDisplayName}";
            GetText((int)Texts.Rank_Text).text = $"순위 : {rank}위";

            GetImage((int)Images.EnemyDescrpition_Image).sprite = data.UnitIcon;

            BindEvent(gameObject, HandleClickEvent, Shared.EUIEvent.Click);
        }
        private void HandleClickEvent(PointerEventData uievt)
        {
            Debug.Log("ㄴㅁ어냐");
            UIEvent.UnitChoiceUIEvent.isClick = this;
            UnitChoiceUIEvent evt = UIEvent.UnitChoiceUIEvent;
            _uiEvenetChannelSO.RaiseEvent(evt);
        }
        public void SetChoiceState(bool isChoice)
        {
            _background.DOFade(isChoice == true ? 1 : 0.3f, 0.2f);
        }
    }
}
