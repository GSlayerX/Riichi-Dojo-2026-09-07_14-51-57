using UnityEngine;
using UnityEngine.UI;

namespace RiichiDojo.Mahjong
{
    [RequireComponent(typeof(GridLayoutGroup))]
    public class HandLayout : MonoBehaviour
    {
        private const int TileCount = 13;
        private const int TwoRowColumnCount = 7;

        private const float MaxTileWidth = 100f;
        private const float MinTileWidth = 80f;

        private const float TileAspectRatio = 3f / 4f;

        [SerializeField]
        private float _horizontalPadding = 20f;

        [SerializeField]
        private float _verticalPadding = 10f;

        [SerializeField]
        private float _horizontalSpacing = 5f;

        [SerializeField]
        private float _verticalSpacing = 5f;

        private GridLayoutGroup _layoutGroup;

        private void Awake()
        {
            _layoutGroup =
                GetComponent<GridLayoutGroup>();
        }

        private void OnRectTransformDimensionsChange()
        {
            UpdateTileSize();
        }

        private void OnTransformChildrenChanged()
        {
            UpdateTileSize();
        }

        private void Start()
        {
            UpdateTileSize();
        }

        private void UpdateTileSize()
        {
            if (_layoutGroup == null)
            {
                return;
            }

            RectTransform rectTransform =
                (RectTransform)transform;

            float width =
                rectTransform.rect.width;

            if (width <= 0f)
            {
                return;
            }

            float oneRowTileWidth =
                CalculateTileWidth(
                    width,
                    TileCount);

            bool useTwoRows =
                oneRowTileWidth < MinTileWidth;

            int columnCount =
                useTwoRows
                    ? TwoRowColumnCount
                    : TileCount;

            float tileWidth =
                CalculateTileWidth(
                    width,
                    columnCount);

            tileWidth =
                Mathf.Min(
                    MaxTileWidth,
                    tileWidth);

            float tileHeight =
                tileWidth / TileAspectRatio;

            _layoutGroup.padding.left =
                Mathf.RoundToInt(
                    _horizontalPadding);

            _layoutGroup.padding.right =
                Mathf.RoundToInt(
                    _horizontalPadding);

            _layoutGroup.padding.top =
                Mathf.RoundToInt(
                    _verticalPadding);

            _layoutGroup.padding.bottom =
                Mathf.RoundToInt(
                    _verticalPadding);

            _layoutGroup.spacing =
                new Vector2(
                    _horizontalSpacing,
                    _verticalSpacing);

            _layoutGroup.constraint =
                GridLayoutGroup.Constraint.FixedColumnCount;

            _layoutGroup.constraintCount =
                columnCount;

            _layoutGroup.cellSize =
                new Vector2(
                    tileWidth,
                    tileHeight);

            _layoutGroup.startCorner =
                GridLayoutGroup.Corner.UpperLeft;

            _layoutGroup.startAxis =
                GridLayoutGroup.Axis.Horizontal;

            _layoutGroup.childAlignment =
                TextAnchor.UpperCenter;
        }

        private float CalculateTileWidth(
            float containerWidth,
            int columnCount)
        {
            float availableWidth =
                containerWidth
                - _horizontalPadding * 2f
                - _horizontalSpacing *
                    (columnCount - 1);

            if (availableWidth <= 0f)
            {
                return 0f;
            }

            return availableWidth / columnCount;
        }
    }
}