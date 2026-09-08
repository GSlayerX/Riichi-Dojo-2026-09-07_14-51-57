using UnityEngine;

namespace RiichiDojo.Mahjong
{
    [CreateAssetMenu(
        fileName = "TileTheme",
        menuName = "Riichi Dojo/Mahjong/Tile Theme")]
    public class TileThemeData : ScriptableObject
    {
        [SerializeField]
        private string _themeName;

        [SerializeField]
        private Sprite _background;

        [SerializeField]
        private Sprite[] _sprites = new Sprite[34];

        public string ThemeName => _themeName;

        public Sprite Background => _background;

        public Sprite GetSprite(TileType tile)
        {
            return _sprites[(int)tile];
        }
    }
}