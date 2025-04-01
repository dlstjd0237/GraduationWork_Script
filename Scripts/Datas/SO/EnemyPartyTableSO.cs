using System.Collections.Generic;
using UnityEngine;

namespace BIS.Data
{
    [CreateAssetMenu(menuName = "BIS/SO/Data/EnemyPartyTableSO")]
    public class EnemyPartyTableSO : ScriptableObject
    {
        [SerializeField] private List<EnemyPartySO> _enemyPartySOTable; public List<EnemyPartySO> EnemypartySOTable { get { return _enemyPartySOTable; } }

    }
}
