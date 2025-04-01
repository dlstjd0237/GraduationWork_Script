using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace BIS.UI
{
    public class Tabbar
    {

        private const string _choiceKey = "choice";

        private VisualElement _root;


        private VisualElement _tabbarButton;
        public VisualElement TabbarButton => _tabbarButton;


        private VisualElement _tabbarContain;
        public VisualElement TabbarContain => _tabbarContain;

        private List<Button> _tabbarList;
        private List<VisualElement> _tabbarContainList;


        public Tabbar(VisualElement root, TabbarSOList tabbarSO)
        {
            _root = root;

            for (int i = 0; i < tabbarSO.List.Count; ++i)
            {
                _tabbarList[i] = root.Q<Button>($"{tabbarSO.List[i].ToString().ToLower()}_tab_button");
                _tabbarContainList[i] = root.Q<VisualElement>($"main_{tabbarSO.List[i].ToString().ToLower()}_contain-box");

                _tabbarList[i].RegisterCallback<ClickEvent>((evt) =>
                {
                    AllTabbarRemoveToClass(_choiceKey);
                    _tabbarList[i].AddToClassList(_choiceKey);
                    _tabbarContainList[i].AddToClassList(_choiceKey);
                });
            }
        }

        public void AllTabbarAddToClass(string className)
        {
            for (int i = 0; i < _tabbarList.Count; ++i)
            {
                _tabbarList[i].AddToClassList(className);
            }
        }
        public void AllTabbarRemoveToClass(string className)
        {
            for (int i = 0; i < _tabbarList.Count; ++i)
            {
                _tabbarList[i].RemoveFromClassList(className);
            }
        }

    }
}
