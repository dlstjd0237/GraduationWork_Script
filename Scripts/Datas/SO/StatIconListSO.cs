using System;
using UnityEngine;
using Main.Runtime.Core.StatSystem;
using System.Collections.Generic;

namespace BIS.Data
{
    [Serializable]
    public struct StatIconData
    {
        public StatSO statSO;
        public Sprite icon;
    }
    [CreateAssetMenu(menuName = "BIS/SO/Data/StatIconList")]
    public class StatIconListSO : ScriptableObject
    {
        [SerializeField] private List<StatIconData> _statIconList; public List<StatIconData> StatIconList { get { return _statIconList; } }
    }
}
