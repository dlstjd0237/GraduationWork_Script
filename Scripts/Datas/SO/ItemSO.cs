using System;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace BIS.Data
{
    public class ItemSO : ScriptableObject
    {
        [SerializeField] private string _itemName; public string ItemName { get { return _itemName; } set { _itemName = value; } }
        [SerializeField] private string _itemID; public string ItemID { get { return _itemID; } set { _itemID = value; } }
        [SerializeField] private Sprite _itemIcon; public Sprite ItemIcon { get { return _itemIcon; } set { _itemIcon = value; } }
        [TextArea] [SerializeField] private string _itemDescription; public string ItemDescription { get { return _itemDescription; } set { _itemDescription = value; } }
        [SerializeField] private int _itemPrice; public int ItemPrice { get { return _itemPrice; } set { _itemPrice = value; } }


#if UNITY_EDITOR
        private void OnValidate()
        {


            if (name != $"[{_itemName}]Item")
            {
                string newName = $"[{_itemName}]Item";

                // 실제로 Asset의 이름을 변경
                //AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(this), newName);

                // AssetDatabase를 강제로 갱신
                // AssetDatabase.SaveAssets();
                // AssetDatabase.Refresh();

                string path = AssetDatabase.GetAssetPath(this);
                _itemID = AssetDatabase.AssetPathToGUID(path);
            }
        }
#endif

    }
}
