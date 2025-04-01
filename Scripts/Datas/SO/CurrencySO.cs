using System;
#if UNITY_EDITOR    
using UnityEditor;
#endif
using UnityEngine;
using BIS.Shared;
namespace BIS.Data
{
    [CreateAssetMenu(menuName = "BIS/SO/Data/Currency")]
    public class CurrencySO : ScriptableObject
    {
        [SerializeField] private string _currencyName; public string CurrencyName { get { return _currencyName; } }
        [SerializeField] private string _displayName; public string DisplayName { get { return _displayName; } }
        [SerializeField] private string _currencyId; public string CurrencyId { get { return _currencyId; } }
        [SerializeField] private Sprite _currencyIcon; public Sprite CurrencyIcon { get { return _currencyIcon; } }
        [SerializeField] private int _currentAmmount = 0; public int CurrentAmmount { get { return _currentAmmount; } }

        public event Action<int> ValueChangeEvent;

        public void ChangeValue(int newValue)
        {
            if (newValue != _currentAmmount)
                ValueChangeEvent?.Invoke(newValue);
            _currentAmmount = newValue;
        }

        /// <summary>
        /// ����
        /// </summary>t
        /// <param name="price">����</param>
        /// <returns></returns>
        public PurchaseData Purchase(int price)
        {
            if (price > _currentAmmount) //�ݾ� ����
                return new PurchaseData() { isPurchasable = false, restrictionMessage = Define.SRestrictionMessage };
            ChangeValue(_currentAmmount - price);
            return new PurchaseData() { isPurchasable = true };
        }

        public void AddAmmount(int ammount)
        {
            _currentAmmount += ammount;
            ChangeValue(_currentAmmount);
        }


#if UNITY_EDITOR

        private void OnValidate()
        {
            if (name != $"{_currencyName}[{_displayName}]")
            {
                string newName = $"{_currencyName}[{_displayName}]";

                // ������ Asset�� �̸��� ����
                AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(this), newName);

                // AssetDatabase�� ������ ����
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                string path = AssetDatabase.GetAssetPath(this);
                _currencyId = AssetDatabase.AssetPathToGUID(path);
            }
        }

#endif
    }
}
