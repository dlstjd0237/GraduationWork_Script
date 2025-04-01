using BIS.Data;

namespace BIS.Shared.Interface
{
    public interface IAchievementChoiceable 
    {
        public AchievementSO Data { get; }
        void SetChoiceState(bool isChoice);
    }
}
