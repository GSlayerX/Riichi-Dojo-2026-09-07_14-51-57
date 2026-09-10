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
    private TMP_Dropdown _trainingModeDropdown;
    
    [SerializeField]
    private TileThemeDatabase _themeDatabase;
    
    [SerializeField]
    private string _themeName = "White";

    [SerializeField]
    private TrainingModeData[] _trainingModes;
    
    private TrainingModeData _currentTrainingMode;
    private TrainingQuestion _currentQuestion;
    private readonly AnswerChecker _answerChecker = new();
    
    private void Start()
    {
        InitializeTrainingModeDropdown();

        _currentTrainingMode = _trainingModes[0];

        LoadQuestion();

        _trainingModeDropdown.onValueChanged.AddListener(
            OnTrainingModeChanged);
    }
    
    private void InitializeTrainingModeDropdown()
    {
        _trainingModeDropdown.ClearOptions();

        var options =
            new System.Collections.Generic.List<string>();

        foreach (TrainingModeData mode in _trainingModes)
        {
            options.Add(mode.DisplayName);
        }

        _trainingModeDropdown.AddOptions(options);
    }
    
    private void LoadQuestion()
    {
        var generator = new HandGenerator();

        if (_currentTrainingMode.IsAll)
        {
            _currentQuestion = generator.GenerateQuestion();
        }
        else
        {
            _currentQuestion =
                generator.GenerateQuestion(
                    _currentTrainingMode.Pattern);
        }

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
    
    private void OnTrainingModeChanged(int value)
    {
        _currentTrainingMode = _trainingModes[value];

        NextQuestion();
    }
}