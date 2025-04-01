using BIS.Shared;
using System.Collections.Generic;
using UnityEngine;

namespace BIS.Data
{
    [CreateAssetMenu(menuName = "BIS/SO/Data/SpawnData")]
    public class SpawnDataSO : ScriptableObject
    {
        [SerializeField] private List<SpawnDataList> _spawnDataList; public List<SpawnDataList> SpawnDataList { get { return _spawnDataList; } }
        [SerializeField] private EScene _spawnScene; public EScene SpawnScene { get { return _spawnScene; } }
    }
}
