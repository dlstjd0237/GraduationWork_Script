using UnityEngine;
using BIS.Manager;
using BIS.Core.Utility;
using System.Collections;
using System;
using BIS.Shared.Interface;

namespace BIS.UI.Popup
{
    public abstract class PopupUI : UIBase, IPopupUI
    {
        public GameObject PopupGO => gameObject;

        public override bool Init()
        {
            if (base.Init() == false)
                return false;


            Managers.UI.SetCanvas(gameObject);
            return true;
        }

        public virtual void ClosePopup(Action callBack = null)
        {
            Util.UIFadeOut(gameObject, true, 0.2f, callBack);
            Managers.Save.SaveGame();
        }

        public virtual void OpenPopup()
        {
            Util.UIFadeOut(gameObject, false);
            StartCoroutine(LoadGameCoroutine());
        }

        private IEnumerator LoadGameCoroutine()
        {
            yield return new WaitForSeconds(0.1f);
            Managers.Save.LoadGame();
        }
    }
}
