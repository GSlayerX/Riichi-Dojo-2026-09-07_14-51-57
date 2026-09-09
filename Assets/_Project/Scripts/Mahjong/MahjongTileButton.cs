using UnityEngine;
using UnityEngine.UI;

namespace RiichiDojo.Mahjong
{
    [RequireComponent(typeof(Button))]
    public class MahjongTileButton : MonoBehaviour
    {
        [SerializeField]
        private MahjongTileUI _tileUI;

        private Button _button;

        private TileType _tile;

        public TileType Tile => _tile;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        public void Initialize(TileType tile)
        {
            _tile = tile;
            _tileUI.SetTile(tile);
        }
        
        public void SetTheme(TileThemeData theme)
        {
            _tileUI.SetTheme(theme);
        }
    }
}