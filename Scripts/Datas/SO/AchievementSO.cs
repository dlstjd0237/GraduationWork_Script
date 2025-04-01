using UnityEngine;
#if UNITY_EDITOR    

using UnityEditor;

#endif
using BIS.Shared;
using System;

namespace BIS.Data
{
    [CreateAssetMenu(menuName = "BIS/SO/Data/Achievement")]
    public class AchievementSO : ScriptableObject
    {
        [SerializeField] private string _achievementName; public string AchievementName { get { return _achievementName; } }
        [SerializeField] private string _achievementID; public string AhcievementID { get { return _achievementID; } }
        [SerializeField] private Sprite _icon; public Sprite Icon { get { return _icon; } }
        [SerializeField] private ERarity _rare; public ERarity Rare { get { return _rare; } }
        [Tooltip("How to obtain")] [TextArea(3, 4)] [SerializeField] private string _description; public string Description { get { return _description; } }
        [SerializeField] private bool _isAchieve; public bool IsAchieve
        {
            get
            {
                return _isAchieve;
            }
            set
            {
                if (_isAchieve != value) // �������� ����� ȣ��
                    AchieveChangeEvent?.Invoke(value);
                _isAchieve = value;

            }
        }
        public event Action<bool> AchieveChangeEvent;

#if UNITY_EDITOR

        private void OnValidate()
        {


            if (name != $"[{_achievementName}]Achieve")
            {
                string newName = $"[{_achievementName}]Achieve";

                // ������ Asset�� �̸��� ����
                AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(this), newName);

                // AssetDatabase�� ������ ����
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                string path = AssetDatabase.GetAssetPath(this);
                _achievementID = AssetDatabase.AssetPathToGUID(path);
            }
        }

#endif
    }
}
