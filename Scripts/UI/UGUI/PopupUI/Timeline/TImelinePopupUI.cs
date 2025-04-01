using UnityEngine;

namespace BIS.UI.Popup
{
    public class TImelinePopupUI : PopupUI
    {
        private enum Images
        {
            SkipProgrresbar_Image
        }

        public override bool Init()
        {
            if (base.Init() == false)
                return false;

            // ==== UI Binding ====
            BindImages(typeof(Images));
            // ====================


            return true;
        }
    }
}
