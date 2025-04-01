using BIS.Data;
using System;
using System.IO;
#if UNITY_EDITOR

using UnityEditor;

#endif
using UnityEngine;

namespace BIS.Editors
{
#if UNITY_EDITOR
    [CustomEditor(typeof(CurrencySO))]
    public class CurrencySOEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space(); // UI 간격 추가

            if (GUILayout.Button("구조체 생성")) // 여기에 원하는 버튼 이름 입력
            {
                CreateStructScript();
            }
            if (GUILayout.Button("SaveID 생성"))
            {
                CreateSaveID();
            }
        }

        private void CreateSaveID()
        {
            CurrencySO currencySO = (CurrencySO)target;

            string folderPath = "Assets/00Work/BIS/@Resources/SO/SaveID/Currencys/";
            string assetPath = $"{folderPath}{currencySO.CurrencyName}.asset";

            SaveIDSO CheckIDSO = AssetDatabase.LoadAssetAtPath<SaveIDSO>(assetPath);
            //폴더에 이미 똑같은 이름의 SO가 존재하는지 확인 
            if (CheckIDSO == null)
            {
                string guid = AssetDatabase.AssetPathToGUID(assetPath);
                SaveIDSO saveIDSO = ScriptableObject.CreateInstance<SaveIDSO>();
                saveIDSO.saveName = currencySO.CurrencyName;
                saveIDSO.saveId = guid;

                AssetDatabase.CreateAsset(saveIDSO, assetPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        private void CreateStructScript()
        {
            CurrencySO currencySO = (CurrencySO)target;

            string relativePath = "Assets/00Work/BIS/@Scripts/Datas/Structs/";
            string fileName = currencySO.CurrencyName + "Data.cs";
            string fullPath = Path.Combine(Application.dataPath, "../", relativePath, fileName);

            string directoryPath = Path.GetDirectoryName(fullPath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            //스크립트 내용
            string scriptContent =
            $@"using UnityEngine;

namespace BIS.Data
{{
    [System.Serializable]
    public struct {currencySO.CurrencyName}Data
    {{
        public int CurrencyAmmount;
    }}
}}";

            File.WriteAllText(fullPath, scriptContent);

            AssetDatabase.Refresh();
        }
    }
#endif
}
