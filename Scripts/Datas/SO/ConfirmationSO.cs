using UnityEngine;

namespace BIS.Data
{
    [CreateAssetMenu(menuName = "BIS/SO/Data/Confirmation")]
    public class ConfirmationSO : ScriptableObject
    {
        [SerializeField] [TextArea(3, 4)] private string _description; public string Description { get { return _description; } }
    }
}
