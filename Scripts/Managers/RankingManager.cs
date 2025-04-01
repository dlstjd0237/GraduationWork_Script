using UnityEngine;
using BIS.Data;

namespace BIS.Manager
{
    public class RankingManager
    {
        private RankData _data;
        public RankData Data => _data;

        public RankingManager()
        {
            _data = new RankData();
            _data.CurrentRank = 9999;
        }
        public void ChangeRank(int rank)
        {
            _data.CurrentRank = rank;
        }
        public int GetEnemyRandomRank() => Random.Range(Mathf.Max(_data.CurrentRank - 500, 1), _data.CurrentRank);
        public string GetPlayerRank() => $"ÇöÀç ·©Å· : {_data.CurrentRank}";
    }
}
