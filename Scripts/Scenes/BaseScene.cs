using BIS.Shared;
using BIS.Shared.Interface;
using BIS.Initializes;

namespace BIS.Scenes
{
    public class BaseScene : InitBase, IScene
    {
        public EScene SceneType { get; set; } = EScene.Unknown;

        public override bool Init()
        {
            if (base.Init() == false)
                return false;


            return true;
        }
    }
}
