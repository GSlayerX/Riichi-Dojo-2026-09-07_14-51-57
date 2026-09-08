using UnityEngine;
using RiichiDojo.Mahjong;

public class TrainingController : MonoBehaviour
{
    [SerializeField]
    private MahjongTileUI _tilePrefab;

    [SerializeField]
    private Transform _handContainer;

    [SerializeField]
    private TileThemeDatabase _themeDatabase;
    
    [SerializeField]
    private string _themeName = "White";
    
    private void Start()
    {
        var generator = new HandGenerator();
        var question = generator.GenerateQuestion();

        TileThemeData theme =
            _themeDatabase.GetTheme(_themeName);

        if (theme == null)
        {
            Debug.LogError(
                $"Tile theme '{_themeName}' was not found.");
            return;
        }

        foreach (TileType tile in question.Hand.Tiles)
        {
            MahjongTileUI tileUI =
                Instantiate(_tilePrefab, _handContainer);

            tileUI.SetTile(tile);
            tileUI.SetTheme(theme);
        }
    }
}