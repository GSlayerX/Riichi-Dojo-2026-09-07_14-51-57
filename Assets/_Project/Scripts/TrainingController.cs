using UnityEngine;
using RiichiDojo.Mahjong;

public class TrainingController : MonoBehaviour
{
    [SerializeField]
    private MahjongTileUI _tile;

    private void Start()
    {
        _tile.SetTile(TileType.Man1);
    }
}