#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

namespace BIS.Data
{
    [CreateAssetMenu(menuName = "BIS/SO/Data/UnitName")]
    public class UnitSO : ScriptableObject
    {
        [Header("Set in editor only")]
        [field: SerializeField]
        public PoolTypeSO UnitPoolType { get; private set; }

        [SerializeField] private string _unitDisplayName;

        public string UnitDisplayName
        {
            get { return _unitDisplayName; }
        }

        [SerializeField] private string _unitDescription;

        public string UnitDescription
        {
            get { return _unitDescription; }
        }

        [SerializeField] private Sprite _unityIcon;

        public Sprite UnitIcon
        {
            get { return _unityIcon; }
        }


        [Header("RunTime Setting")]
        [Tooltip("don't touch editor mode pls")]
        [HideInInspector]
        public int Rank { get; set; }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (name != $"[{_unitDisplayName}]")
            {
                string newName = $"[{_unitDisplayName}]";

                // ������ Asset�� �̸��� ����
                AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(this), newName);

                // AssetDatabase�� ������ ����
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }
#endif
    }
}