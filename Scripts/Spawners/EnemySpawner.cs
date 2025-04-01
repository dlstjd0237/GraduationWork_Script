using BIS.Data;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using BIS.Manager;

namespace BIS.Spawners
{
    //enemy�� Ǯ���� �Ǿ��ִٸ� Ǯ���� ����� �ڵ�� ����
    public class EnemySpawner : MonoBehaviour
    {
        [HideInInspector] public List<Transform> SpawnTrmList { get; private set; }

        private void Awake()
        {
            SpawnTrmList = new List<Transform>();

            SpawnTrmList = transform.GetComponentsInChildren<Transform>().ToList();
        }

        // public void EnemySpawn(SpawnDataSO data)
        // {
        //     int spawnDataListCount = data.SpawnDataList.Count;
        //     for (int i = 0; i < spawnDataListCount; ++i)
        //     {
        //         int spawnCount = data.SpawnDataList[i].spawnData.Count;
        //         for (int j = 0; j < spawnCount; ++j)
        //         {
        //             for (int k = 0; k < data.SpawnDataList[i].spawnData[j].spawnAmount; ++k)
        //             {
        //                 string key = data.SpawnDataList[i].spawnData[j].spawnUnit.UnitName;
        //                 Transform spawnTrm = SpawnTrmList[Random.Range(0, SpawnTrmList.Count)];
        //                 Managers.Resource.Instantiate(key).transform.position = spawnTrm.localPosition;
        //             }
        //         }
        //     }
        // }
    }
}