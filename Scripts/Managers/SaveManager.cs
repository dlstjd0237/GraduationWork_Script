using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using BIS.Shared.Interface;
using BIS.Data;

namespace BIS.Manager
{
    [Serializable]
    public struct SaveData
    {
        public string id;
        public string data;
    }

    [Serializable]
    public struct DataCollection
    {
        public List<SaveData> dataList;
    }

    public class SaveManager
    {
        [SerializeField] private string _saveDataKey = "saveData"; // 저장할 키

        private List<SaveData> _unUsedData = new List<SaveData>();

        public void LoadGame() // 게임 로드
        {
            // PlayerPrefs에서 저장된 데이터를 가져옴
            // 없으면 빈 문자열을 반환
            string loadData = PlayerPrefs.GetString(_saveDataKey, string.Empty);

            // 데이터 불러오기
            RestoreData(loadData);
        }

        private void RestoreData(string loadData) // 데이터 복원
        {
            IEnumerable<ISavable> savableObjects
               = Managers.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<ISavable>();
            DataCollection collection = string.IsNullOrEmpty(loadData) ? new DataCollection() : JsonUtility.FromJson<DataCollection>(loadData);

            _unUsedData.Clear();
            if (collection.dataList != null)
            {
                foreach (SaveData saveData in collection.dataList)
                {
                    ISavable savable = savableObjects.FirstOrDefault(x => x.IdData.saveId == saveData.id);
                    if (savable != null)
                    {
                        savable.RestoreData(saveData.data);
                    }
                    else
                    {
                        _unUsedData.Add(saveData); //현재씬에 복구할 게 없다면 버리지 말고 미사용 데이터에 넣어두어야 한다.
                    }
                }
            }
        }

        public void SaveGame() // 게임 저장
        {
            string dataToSave = GetDataToSave();
            PlayerPrefs.SetString(_saveDataKey, dataToSave);
        }

        private string GetDataToSave() // 저장할 데이터 가져오기
        {
            IEnumerable<ISavable> savableObjects
                = Managers.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<ISavable>();

            List<SaveData> toSaveData = new List<SaveData>();
            foreach (ISavable savable in savableObjects)
            {
                toSaveData.Add(new SaveData { id = savable.IdData.saveId, data = savable.GetSaveData() });
            }

            toSaveData.AddRange(_unUsedData);
            DataCollection collection = new DataCollection { dataList = toSaveData };
            return JsonUtility.ToJson(collection);
        }
    }
}
