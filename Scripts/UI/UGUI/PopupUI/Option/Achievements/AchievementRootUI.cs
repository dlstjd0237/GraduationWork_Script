using System.Collections.Generic;
using UnityEngine;
using Main.Runtime.Core.Events;
using BIS.UI;
using BIS.Data;
using BIS.Core.Utility;
using BIS.Manager;
using BIS.UI.Popup;
using BIS.Events;
using System;
using BIS.Shared.Interface;

public class AchievementRootUI : UIBase
{
    [SerializeField] private AchievementTalbeSO _dataList;
    private GameEventChannelSO _uiEvenetChannelSO = default;

    private Transform _achievementUIRoot;
    private List<AchievementUI> _achievementList;
    private AchievementDescriptionUI _description;
    private IAchievementChoiceable _choiceUI;

    public override bool Init()
    {
        _uiEvenetChannelSO = Managers.Resource.Load<GameEventChannelSO>("UIEventChannelSO");
        _achievementList = new List<AchievementUI>();

        _achievementUIRoot = Util.FindChild<Transform>(gameObject, "Content", true);
        (_achievementUIRoot as RectTransform).SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,
            (_dataList.List.Count * 150) + ((_dataList.List.Count - 1) * 50));

        _description = Util.FindChild<AchievementDescriptionUI>(gameObject, "Description", true);

        for (int i = 0; i < _dataList.List.Count; ++i)
        {
            var go = Managers.Resource.Instantiate("Achievement", _achievementUIRoot);
            AchievementUI ui = go.GetComponent<AchievementUI>();
            ui.SetUp(_dataList.List[i]);
            ui.SetChoiceState(false);
            _achievementList.Add(ui);
        }

        _choiceUI = _achievementList[_dataList.List.Count - 1];
        _choiceUI.SetChoiceState(true);
        _description.SetUpDescription(_choiceUI.Data);

        _uiEvenetChannelSO.AddListener<AchievementUIEvent>(HandleUIEvenet);

        return false;
    }

    private void OnDestroy()
    {
        _uiEvenetChannelSO.RemoveListener<AchievementUIEvent>(HandleUIEvenet);
    }

    private void HandleUIEvenet(AchievementUIEvent evt)
    {
        _choiceUI.SetChoiceState(false);
        _choiceUI = evt.isClick;
        _choiceUI.SetChoiceState(true);
        _description.SetUpDescription(_choiceUI.Data);
    }
}