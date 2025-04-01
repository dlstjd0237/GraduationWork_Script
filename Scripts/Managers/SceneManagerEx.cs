using System;

using UnityEngine;
using UnityEngine.SceneManagement;

using BIS.Shared;

namespace BIS.Manager
{
    public class SceneManagerEx
    {

        public void LoadScene(EScene type)
        {
            SceneManager.LoadScene(GetSceneName(type));
        }

        public void LoadScene(string SceneName)
        {
            Debug.Log("이곳 이넘으로 바꾸게 바꿔야함");
            SceneManager.LoadScene(SceneName);
        }


        private string GetSceneName(EScene type)
        {
            string name = Enum.GetName(typeof(EScene), type);
            return name;
        }
    }
}
