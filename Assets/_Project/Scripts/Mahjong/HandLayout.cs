using UnityEngine;
using UnityEngine.UI;

namespace RiichiDojo.Mahjong
{
    [RequireComponent(typeof(HorizontalLayoutGroup))]
    public class HandLayout : MonoBehaviour
    {
        private const int TileCount = 13;
        private const float TileAspectRatio = 3f / 4f;

        [SerializeField]
        private float _horizontalPadding = 20f;

        [SerializeField]
        private float _spacing = 5f;

        private HorizontalLayoutGroup _layoutGroup;

        private void Awake()
        {
            _layoutGroup = GetComponent<HorizontalLayoutGroup>();
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

            float availableWidth =
                rectTransform.rect.width
                - _horizontalPadding * 2f
                - _spacing * (TileCount - 1);

            if (availableWidth <= 0f)
            {
                return;
            }

            float tileWidth =
                availableWidth / TileCount;

            float tileHeight =
                tileWidth / TileAspectRatio;

            foreach (RectTransform child in rectTransform)
            {
                LayoutElement layoutElement =
                    child.GetComponent<LayoutElement>();

                if (layoutElement == null)
                {
                    layoutElement =
                        child.gameObject.AddComponent<LayoutElement>();
                }

                layoutElement.preferredWidth = tileWidth;
                layoutElement.preferredHeight = tileHeight;
            }

            _layoutGroup.padding.left =
                Mathf.RoundToInt(_horizontalPadding);

            _layoutGroup.padding.right =
                Mathf.RoundToInt(_horizontalPadding);

            _layoutGroup.spacing = _spacing;
        }
    }
}