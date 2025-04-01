using System;
using UnityEngine;

namespace BIS.Data
{
    [Serializable]
    public struct SpawnData
    {
        public UnitSO spawnUnit;
        [Min(1)] public int spawnAmount;
    }
}
