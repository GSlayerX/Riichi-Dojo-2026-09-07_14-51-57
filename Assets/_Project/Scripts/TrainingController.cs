using UnityEngine;
using RiichiDojo.Mahjong;
using TMPro;

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

    [SerializeField]
    private TMP_Dropdown _trainingDifficultyDropdown;

    [SerializeField]
    private TrainingDifficultyData[] _trainingDifficulties;
    
    private TrainingQuestion _currentQuestion;
    private TrainingDifficultyData _currentTrainingDifficulty;
    private readonly AnswerChecker _answerChecker = new();
    
    private void Start()
    {
        InitializeTrainingDifficultyDropdown();

        _currentTrainingDifficulty = _trainingDifficulties[0];

        LoadQuestion();

        _trainingDifficultyDropdown.onValueChanged.AddListener(
            OnTrainingDifficultyChanged);
    }
    
    private void LoadQuestion()
    {
        var generator = new HandGenerator();

        HandGenerationSettings settings =
            _currentTrainingDifficulty.GetGenerationSettings();

        _currentQuestion =
            generator.GenerateQuestion(settings);

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

        _resultText.text = string.Empty;
    }
    
    public void CheckAnswer()
    {
        bool isCorrect =
            _answerChecker.IsCorrect(
                _currentQuestion,
                _tileSelector.SelectedTiles);
        
        _tileSelector.SetSelectionEnabled(false);

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
    
    public void NextQuestion()
    {
        foreach (Transform child in _handContainer)
        {
            Destroy(child.gameObject);
        }

        LoadQuestion();
    }
    
    private void InitializeTrainingDifficultyDropdown()
    {
        _trainingDifficultyDropdown.ClearOptions();

        var options =
            new System.Collections.Generic.List<string>();

        foreach (TrainingDifficultyData difficulty in _trainingDifficulties)
        {
            options.Add(difficulty.DisplayName);
        }

        _trainingDifficultyDropdown.AddOptions(options);
    }
    
    private void OnTrainingDifficultyChanged(int value)
    {
        _currentTrainingDifficulty =
            _trainingDifficulties[value];

        NextQuestion();
    }
}