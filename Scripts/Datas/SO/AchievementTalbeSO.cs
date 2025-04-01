using System.Collections.Generic;
using UnityEngine;

namespace BIS.Data
{
    [CreateAssetMenu(menuName = "BIS/SO/Data/AchievementTable")]

    public class AchievementTalbeSO : ScriptableObject
    {
        [SerializeField] private List<AchievementSO> _list; public List<AchievementSO> List { get { return _list; } }
    }
}
