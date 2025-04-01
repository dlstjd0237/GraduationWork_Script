using System;
using UnityEngine;

namespace BIS.Shared.Interface
{
    public interface IPopupUI
    {
        GameObject PopupGO { get;}

        bool Init();

        void ClosePopup(Action callBack = null);

        void OpenPopup();
    }
}
