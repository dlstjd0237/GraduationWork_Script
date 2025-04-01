using BIS.Data;

namespace BIS.Shared.Interface
{
    public interface IUnitChoiceable 
    {
        public UnitSO Data { get; }
        void SetChoiceState(bool isChoice);
    }
}
