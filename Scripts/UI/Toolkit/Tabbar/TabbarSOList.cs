using System.Collections.Generic;
using UnityEngine;

namespace BIS.UI
{
    [CreateAssetMenu(fileName = "TabbarSOList", menuName = "SO/BIS/UI/TabbarSO/List")]
    public class TabbarSOList : ScriptableObject
    {
        [SerializeField] private List<TabbarSO> _list; public List<TabbarSO> List { get { return _list ; } }

    }
}
