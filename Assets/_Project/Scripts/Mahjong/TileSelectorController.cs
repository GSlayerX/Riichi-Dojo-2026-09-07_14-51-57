using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace RiichiDojo.Mahjong
{
    public class TileSelectorController : MonoBehaviour
    {
        [SerializeField]
        private MahjongTileButton _tileButtonPrefab;

        [SerializeField]
        private Transform _container;

        [SerializeField]
        private MahjongTileUI _selectedTilePrefab;

        [SerializeField]
        private Transform _selectedTilesContainer;
        
        [SerializeField]
        private TileThemeData _theme;

        private const int MaxSelectedTiles = 13;

        private readonly Dictionary<TileType, MahjongTileUI> _selectedTiles = new();
        private readonly Dictionary<TileType, MahjongTileButton> _selectedButtons = new();
        private readonly List<MahjongTileUI> _correctAnswerTiles = new();
        
        public IReadOnlyList<TileType> SelectedTiles =>
            _selectedTiles.Keys.ToList();
        
        public void Initialize(MahjongHand hand)
        {
            Clear();

            foreach (TileType tile in hand.GetAvailableSelectionTiles())
            {
                MahjongTileButton tileButton =
                    Instantiate(_tileButtonPrefab, _container);

                tileButton.Initialize(tile);
                tileButton.SetTheme(_theme);

                tileButton.Clicked += OnTileClicked;
            }
        }

        private void OnTileClicked(TileType tile)
        {
            if (_selectedTiles.TryGetValue(tile, out MahjongTileUI selectedTile))
            {
                _selectedTiles.Remove(tile);

                if (_selectedButtons.TryGetValue(tile, out MahjongTileButton button))
                {
                    button.SetSelected(false);
                    _selectedButtons.Remove(tile);
                }

                Destroy(selectedTile.gameObject);
                return;
            }

            if (_selectedTiles.Count >= MaxSelectedTiles)
            {
                return;
            }

            MahjongTileButton selectedButton = FindButton(tile);

            if (selectedButton == null)
            {
                return;
            }

            MahjongTileUI newSelectedTile =
                Instantiate(_selectedTilePrefab, _selectedTilesContainer);

            newSelectedTile.SetTile(tile);
            newSelectedTile.SetTheme(_theme);

            selectedButton.SetSelected(true);

            _selectedTiles.Add(tile, newSelectedTile);
            _selectedButtons.Add(tile, selectedButton);
        }

        public void ClearSelectedTiles()
        {
            foreach (MahjongTileUI selectedTile in _selectedTiles.Values)
            {
                Destroy(selectedTile.gameObject);
            }

            foreach (MahjongTileButton button in _selectedButtons.Values)
            {
                button.SetSelected(false);
            }

            _selectedTiles.Clear();
            _selectedButtons.Clear();
        }
        
        public void ShowCorrectAnswer(IReadOnlyList<TileType> correctTiles)
        {
            ClearCorrectAnswer();

            foreach (TileType tile in correctTiles)
            {
                MahjongTileUI correctTile =
                    Instantiate(
                        _selectedTilePrefab,
                        _selectedTilesContainer);

                correctTile.SetTile(tile);
                correctTile.SetTheme(_theme);

                _correctAnswerTiles.Add(correctTile);
            }
        }
        
        private void Clear()
        {
            ClearSelectedTiles();
            ClearCorrectAnswer();

            foreach (Transform child in _container)
            {
                MahjongTileButton tileButton =
                    child.GetComponent<MahjongTileButton>();

                if (tileButton != null)
                {
                    tileButton.Clicked -= OnTileClicked;
                }

                Destroy(child.gameObject);
            }
        }
        
        private void ClearCorrectAnswer()
        {
            foreach (MahjongTileUI tile in _correctAnswerTiles)
            {
                if (tile != null)
                {
                    Destroy(tile.gameObject);
                }
            }

            _correctAnswerTiles.Clear();
        }
        
        private MahjongTileButton FindButton(TileType tile)
        {
            foreach (Transform child in _container)
            {
                MahjongTileButton button =
                    child.GetComponent<MahjongTileButton>();

                if (button != null && button.Tile == tile)
                {
                    return button;
                }
            }

            return null;
        }
    }
}