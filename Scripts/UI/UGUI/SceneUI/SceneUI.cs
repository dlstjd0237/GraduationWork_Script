using BIS.Manager;
using BIS.UI;
using BIS.Shared.Interface;
namespace BIS.UI.Scenes
{
    public class SceneUI : UIBase, ISceneUI
    {
        public override bool Init()
        {
            if (base.Init() == false)
                return false;


            Managers.UI.SetCanvas(gameObject, false);
            return true;
        }
    }
}
