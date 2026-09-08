using UnityEngine;

namespace RiichiDojo.Mahjong
{
    [CreateAssetMenu(
        fileName = "TileThemeDatabase",
        menuName = "Riichi Dojo/Mahjong/Tile Theme Database")]
    public class TileThemeDatabase : ScriptableObject
    {
        [SerializeField]
        private TileThemeData[] _themes;

        public TileThemeData[] Themes => _themes;

        public TileThemeData GetTheme(int index)
        {
            if (_themes == null || _themes.Length == 0)
            {
                return null;
            }

            if (index < 0 || index >= _themes.Length)
            {
                return null;
            }

            return _themes[index];
        }
        
        public TileThemeData GetTheme(string themeName)
        {
            if (_themes == null)
            {
                return null;
            }

            foreach (TileThemeData theme in _themes)
            {
                if (theme != null && theme.ThemeName == themeName)
                {
                    return theme;
                }
            }

            return null;
        }
    }
}