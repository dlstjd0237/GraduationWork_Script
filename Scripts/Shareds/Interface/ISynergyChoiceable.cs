using KHJ.SO;
using UnityEngine;
namespace BIS.Shared.Interface
{
    public interface ISynergyChoiceable
    {
        public SynergySO ItemData { get; }
        void SetChoiceState(bool isChoice);
        void DataBinding(SynergySO itemSO);
        void PurchaseItem();
    }
}
