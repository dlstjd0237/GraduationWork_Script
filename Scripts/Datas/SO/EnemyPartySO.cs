using System.Collections.Generic;
using UnityEngine;

namespace BIS.Data
{
    [CreateAssetMenu(menuName = "BIS/SO/Data/EnemyPartySO")]
    public class EnemyPartySO : ScriptableObject
    {
        [SerializeField] private UnitSO _mainUnit; public UnitSO MainUnit { get { return _mainUnit; } }
        [SerializeField] private List<SpawnDataList> _unitDatas; public List<SpawnDataList> UnitDatas { get { return _unitDatas; } }


        public int GetEnemyAveRanking()
        {
            int enemyRankAve = 0;
            int enemySpawnCount = 0;
            int enemySpawnWave = _unitDatas.Count;
            for (int i = 0; i < _unitDatas.Count; ++i)
            {
                for (int j = 0; j < _unitDatas[i].spawnData.Count; j++)
                {
                    enemyRankAve += _unitDatas[i].spawnData[j].spawnUnit.Rank;
                    enemySpawnCount++;
                }
            }

            enemyRankAve += _mainUnit.Rank;
            enemySpawnCount++;

            enemyRankAve = enemyRankAve / enemySpawnCount;
            return enemyRankAve;
        }

        public int GetEnemyMaxRanking()
        {
            int enemyRankMax = 100000;
            for (int i = 0; i < _unitDatas.Count; ++i)
            {
                for (int j = 0; j < _unitDatas[i].spawnData.Count; j++)
                {
                    if (_unitDatas[i].spawnData[j].spawnUnit.Rank < enemyRankMax)
                        enemyRankMax = _unitDatas[i].spawnData[j].spawnUnit.Rank;
                }
            }
            return enemyRankMax;
        }

    }
}
