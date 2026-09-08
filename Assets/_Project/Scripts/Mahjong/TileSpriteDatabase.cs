using UnityEngine;

namespace RiichiDojo.Mahjong
{
    public class TileSpriteDatabase : MonoBehaviour
    {
        [SerializeField]
        private Sprite[] _whiteSprites = new Sprite[34];

        [SerializeField]
        private Sprite[] _blackSprites = new Sprite[34];

        [SerializeField]
        private Sprite _whiteBackground;

        [SerializeField]
        private Sprite _blackBackground;
        
        public Sprite GetSprite(TileType tile, TileTheme theme)
        {
            Sprite[] sprites =
                theme == TileTheme.White
                    ? _whiteSprites
                    : _blackSprites;

            return sprites[(int)tile];
        }
        
        public Sprite GetBackground(TileTheme theme)
        {
            return theme == TileTheme.White
                ? _whiteBackground
                : _blackBackground;
        }
    }
}