using UnityEngine;

namespace BIS.Data
{
    [CreateAssetMenu(menuName = "BIS/SO/Data/Information")]
    public class InformationSO : ScriptableObject
    {
        [SerializeField] private string _titleInfo; public string TitleInfo { get { return _titleInfo; } }
        [SerializeField] [TextArea(5, 6)] private string _descriptionInfo; public string DescriptionInfo { get { return _descriptionInfo; } }
    }
}
