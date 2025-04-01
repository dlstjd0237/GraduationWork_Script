using UnityEngine;

using BIS.Shared;

namespace BIS.UI
{
    [CreateAssetMenu(fileName = "TabbarSO", menuName = "SO/BIS/UI/TabbarSO/Value")]
    public class TabbarSO : ScriptableObject
    {
        [SerializeField] private EMainTabbar _buttonKey; public EMainTabbar ButtonKey { get { return _buttonKey; } }
    }
}
