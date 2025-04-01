#if UNITY_EDITOR    

using UnityEditor;

#endif
using UnityEngine;

namespace BIS.Data
{
    [CreateAssetMenu(menuName = "BIS/SO/Data/SaveID")]
    public class SaveIDSO : ScriptableObject
    {
        public string saveId;
        public string saveName;

#if UNITY_EDITOR
        //private void OnValidate()
        //{
        //     AssetDatabase.SaveAssets();
        //     AssetDatabase.Refresh();

        //     string path = AssetDatabase.GetAssetPath(this);
        //     saveId = AssetDatabase.AssetPathToGUID(path);
        //}
#endif

    }
}

