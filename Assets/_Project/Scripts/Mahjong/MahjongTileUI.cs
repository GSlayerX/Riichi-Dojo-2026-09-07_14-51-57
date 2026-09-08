using UnityEngine;
using UnityEngine.UI;

namespace RiichiDojo.Mahjong
{
    public class MahjongTileUI : MonoBehaviour
    {
        [SerializeField]
        private Image _background;

        [SerializeField]
        private Image _symbol;

        [SerializeField]
        private TileSpriteDatabase _spriteDatabase;

        [SerializeField]
        private TileTheme _theme = TileTheme.White;

        public void SetTile(TileType tile)
        {
            _background.sprite =
                _spriteDatabase.GetBackground(_theme);

            _symbol.sprite =
                _spriteDatabase.GetSprite(tile, _theme);
        }
    }
}