using UnityEngine;
using RiichiDojo.Mahjong;

public class TrainingController : MonoBehaviour
{
    [SerializeField]
    private MahjongTileUI _tilePrefab;

    [SerializeField]
    private Transform _handContainer;

    [SerializeField]
    private TileSelectorController _tileSelector;

    [SerializeField]
    private TMPro.TMP_Text _resultText;
    
    [SerializeField]
    private TileThemeDatabase _themeDatabase;
    
    [SerializeField]
    private string _themeName = "White";

    private TrainingQuestion _currentQuestion;
    private readonly AnswerChecker _answerChecker = new();
    
    private void Start()
    {
        var generator = new HandGenerator();
        _currentQuestion = generator.GenerateQuestion();

        TileThemeData theme =
            _themeDatabase.GetTheme(_themeName);

        if (theme == null)
        {
            Debug.LogError(
                $"Tile theme '{_themeName}' was not found.");
            return;
        }

        _tileSelector.Initialize(_currentQuestion.Hand);

        foreach (TileType tile in _currentQuestion.Hand.Tiles)
        {
            MahjongTileUI tileUI =
                Instantiate(_tilePrefab, _handContainer);

            tileUI.SetTile(tile);
            tileUI.SetTheme(theme);
        }
    }
    
    public void CheckAnswer()
    {
        bool isCorrect =
            _answerChecker.IsCorrect(
                _currentQuestion,
                _tileSelector.SelectedTiles);

        if (isCorrect)
        {
            _resultText.text = "Correct!";
        }
        else
        {
            _tileSelector.ShowCorrectAnswer(_currentQuestion.Waits);
            _resultText.text = "Wrong answer";
        }
    }
}