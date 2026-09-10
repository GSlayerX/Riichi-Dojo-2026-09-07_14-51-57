using UnityEngine;
using UnityEngine.UI;

namespace RiichiDojo.Mahjong
{
    [RequireComponent(typeof(HorizontalLayoutGroup))]
    public class SelectedTilesLayout : MonoBehaviour
    {
        private const float MaxTileWidth = 100f;
        private const float TileAspectRatio = 3f / 4f;

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

            int tileCount = rectTransform.childCount;

            if (tileCount == 0)
            {
                return;
            }

            float availableWidth =
                rectTransform.rect.width
                - _layoutGroup.padding.left
                - _layoutGroup.padding.right
                - _layoutGroup.spacing * (tileCount - 1);

            if (availableWidth <= 0f)
            {
                return;
            }

            float tileWidth =
                Mathf.Min(
                    MaxTileWidth,
                    availableWidth / tileCount);

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

                layoutElement.minWidth = tileWidth;
                layoutElement.minHeight = tileHeight;

                layoutElement.preferredWidth = tileWidth;
                layoutElement.preferredHeight = tileHeight;

                layoutElement.flexibleWidth = 0f;
                layoutElement.flexibleHeight = 0f;
            }
        }
    }
}