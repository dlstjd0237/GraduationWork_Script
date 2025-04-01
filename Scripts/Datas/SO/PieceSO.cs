using Main.Runtime.Core.StatSystem;
using UnityEngine;

namespace BIS.Data
{
    [CreateAssetMenu(fileName = "PieceSO", menuName = "BIS/SO/Data/Piece")]
    public class PieceSO : ItemSO
    {
        [SerializeField] private StatSO _stat; public StatSO Stat { get { return _stat; } }
        [SerializeField] private float _addValue; public float AddValue { get { return _addValue; } }
    }
}
