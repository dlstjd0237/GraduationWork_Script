using UnityEngine;

namespace BIS.Data
{
    public struct RankData
    {
        [SerializeField] private int _currentRank;
        public int CurrentRank
        {
            get => _currentRank;
            set => _currentRank = value;
        }
    }
}
