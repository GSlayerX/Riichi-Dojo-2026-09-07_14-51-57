using UnityEngine;

namespace RiichiDojo.Mahjong
{
    public class TileSelectorController : MonoBehaviour
    {
        [SerializeField]
        private MahjongTileButton _tileButtonPrefab;

        [SerializeField]
        private Transform _container;

        [SerializeField]
        private TileThemeData _theme;

        public void Initialize(MahjongHand hand)
        {
            Clear();

            foreach (TileType tile in hand.GetAvailableSelectionTiles())
            {
                MahjongTileButton tileButton =
                    Instantiate(_tileButtonPrefab, _container);

                tileButton.Initialize(tile);
                tileButton.SetTheme(_theme);
            }
        }

        private void Clear()
        {
            foreach (Transform child in _container)
            {
                Destroy(child.gameObject);
            }
        }
    }
}