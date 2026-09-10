using System;
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

        public event Action<TileType> Clicked;

        private void Awake()
        {
            _button = GetComponent<Button>();
            
            _button.onClick.AddListener(OnClicked);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnClicked);
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

        private void OnClicked()
        {
            Clicked?.Invoke(_tile);
        }
        
        public void SetSelected(bool selected)
        {
            _tileUI.SetSelected(selected);
        }
    }
}