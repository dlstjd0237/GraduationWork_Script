using UnityEngine;

using BIS.Manager;
using BIS.Shared;

namespace BIS.Scenes
{
    public class TitleScene : BaseScene
    {
        [SerializeField] private bool _isText = false;
        public override bool Init()
        {

            if (base.Init() == false)
                return false;

            SceneType = EScene.TitleScene;
            StartLoadAssets();

            return true;
        }

        private void StartLoadAssets()//捞率俊辑 积己秦拎具塞
        {
            Managers.Resource.LoadAllAsync<Object>("PreLoad", (key, count, totalCount) =>
            {
                Debug.Log($"{key} {count}/{totalCount}");

                if (count == totalCount)
                {
                    //Managers.UI.ShowPopup<TitlUI>();
                    if (_isText == true)
                        Managers.Scene.LoadScene("BaekGameScene");
                    Debug.Log("Addressable All Load Complete");
                }
            });

        }
    }
}
