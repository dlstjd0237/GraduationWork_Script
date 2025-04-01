using BIS.Data;
using UnityEngine;

namespace BIS.UI.Popup
{
    public class AchievementDescriptionUI : UIBase
    {
        private enum Images
        {
            Description_Icon_Box_Contain
        }

        private enum Texts
        {
            Description_Name_Text,
            Description_Content_Text
        }

        public void SetUpDescription(AchievementSO data)
        {
            if (GetImage((int)Images.Description_Icon_Box_Contain) == null)
            {
                BindImages(typeof(Images));
                BindTexts(typeof(Texts));
            }
            GetImage((int)Images.Description_Icon_Box_Contain).sprite = data.Icon;
            GetText((int)Texts.Description_Name_Text).text = data.AchievementName;
            GetText((int)Texts.Description_Content_Text).text = data.Description;
        }
    }
}

