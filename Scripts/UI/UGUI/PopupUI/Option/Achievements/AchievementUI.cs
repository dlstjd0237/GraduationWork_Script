using System.Reflection;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Main.Runtime.Core.Events;
using BIS.Data;
using BIS.Shared;
using BIS.Events;
using BIS.Shared.Interface;
using BIS.Core.Utility;
using BIS.Manager;

namespace BIS.UI.Popup
{
    public class AchievementUI : UIBase, IAchievementChoiceable
    {
        private enum Images
        {
            Achievement_Icon
        }

        private enum Texts
        {
            Achievement_Name_Text,
            Achievement_Description_Text
        }

        public AchievementSO Data => _data;
        private AchievementSO _data;
        private Image _background;

        //디버그 용으로 true
        private bool _isAchieve = false;
        private Color _defualtColor;
        private GameEventChannelSO _uiEvenetChannelSO = default;


        public override bool Init()
        {
            if (base.Init() == false)
                return false;
            _uiEvenetChannelSO = Managers.Resource.Load<GameEventChannelSO>("UIEventChannelSO");
            _background = GetComponent<Image>();

            BindImages(typeof(Images));
            BindTexts(typeof(Texts));
            BindEvent(gameObject, HandleClickEvent, EUIEvent.Click);

            return true;
        }

        private void HandleClickEvent(PointerEventData uievt)
        {
            UIEvent.AchievementUIEvent.isClick = this;
            AchievementUIEvent evt = UIEvent.AchievementUIEvent;
            _uiEvenetChannelSO.RaiseEvent(evt);
        }

        public void SetUp(AchievementSO so)
        {
            _data = so;

            Type t = typeof(Define);
            FieldInfo field = t.GetField($"C{so.Rare}", BindingFlags.Public | BindingFlags.Static);
            Color color = (Color)field.GetValue(null);
            _background.color = color;

            _defualtColor = color;

            GetImage((int)Images.Achievement_Icon).sprite = so.Icon;
            GetText((int)Texts.Achievement_Name_Text).text = so.AchievementName;
            GetText((int)Texts.Achievement_Description_Text).text = so.Description;

            Accomplish(so.IsAchieve);
            _data.AchieveChangeEvent += Accomplish;
        }

        public void Accomplish(bool achieve)
        {
            _isAchieve = achieve;
            if (achieve == true)
            {
                Color color = _defualtColor;
                color.a = 0.3f;
                _background.color = color;
            }
            else
                _background.color = new Color(0, 0, 0, 0.3f);
        }

        public void SetChoiceState(bool isChoice)
        {
            if (_isAchieve == false)
                return;

            Color color = _background.color;
            color.a = isChoice == true ? 1 : 0.3f;
            _background.color = color;

            if (isChoice == true)
            {
                UIEvent.AchievementUIEvent.choiceAchieve = _data;
                AchievementUIEvent evt = UIEvent.AchievementUIEvent;
                _uiEvenetChannelSO.RaiseEvent(evt);
            }
        }

        private void OnDisable() => _data.AchieveChangeEvent -= (Accomplish);
    }
}