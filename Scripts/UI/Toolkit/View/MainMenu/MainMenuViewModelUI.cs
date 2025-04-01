using System.Collections.Generic;
using UnityEngine;
namespace BIS.UI.MVVM
{
    public class MainMenuViewModelUI : ViewModelBaseUI
    {
        [SerializeField] private TabbarSOList _tabbarSOList;
        private Tabbar _tabbar;
        public override bool Init()
        {
            if (base.Init() == false)
                return false;


            return true;
        }



        protected override void SetDataBindings()
        {
            _tabbar = new Tabbar(_root, _tabbarSOList);
        }

    }
}
