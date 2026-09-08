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
        private TileThemeData _theme;

        private TileType _tile;

        public void SetTile(TileType tile)
        {
            _tile = tile;

            Refresh();
        }

        public void SetTheme(TileThemeData theme)
        {
            _theme = theme;

            Refresh();
        }

        private void Refresh()
        {
            if (_theme == null)
            {
                return;
            }

            _background.sprite = _theme.Background;
            _symbol.sprite = _theme.GetSprite(_tile);
        }
    }
}