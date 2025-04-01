using System.Text;
using UnityEngine;

namespace BIS.Data
{
    [CreateAssetMenu(menuName = "BIS/SO/Data/Theme")]
    public class ThemeSO : ScriptableObject
    {
        [SerializeField] private string _themeName; public string ThemeName { get { return _themeName; } }
        [SerializeField] private string _themeDisplayName; public string ThemeDisplayName { get { return _themeDisplayName; } }
        [SerializeField] private string _themeDescription; public string ThemeDescription { get { return _themeDescription; } }
        [SerializeField] private Sprite _themeImage; public Sprite ThemeImage { get { return _themeImage; } }


    }
}
