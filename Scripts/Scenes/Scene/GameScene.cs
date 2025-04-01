using UnityEngine;
using BIS.Shared;

namespace BIS.Scenes
{
    public class GameScene : BaseScene
    {
        public override bool Init()
        {
            if (base.Init() == false)
                return false;


            SceneType = EScene.GameScene;


            return true;
        }
    }
}
