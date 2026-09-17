using UnityEngine;
using RiichiDojo.Mahjong;

[CreateAssetMenu(
    fileName = "TrainingDifficulty",
    menuName = "Riichi Dojo/Training Difficulty")]
public class TrainingDifficultyData : ScriptableObject
{
    [SerializeField]
    private string _id;
    
    [SerializeField]
    private string _displayName;

    [SerializeField]
    [Min(1)]
    private int _numberOfSuits;

    [Header("Honor Weights")]

    [SerializeField]
    [Min(0)]
    private int _noHonorsWeight;

    [SerializeField]
    [Min(0)]
    private int _windsWeight;

    [SerializeField]
    [Min(0)]
    private int _dragonsWeight;

    [SerializeField]
    [Min(0)]
    private int _windsAndDragonsWeight;

    [Header("Special Hand Weights")]

    [SerializeField]
    [Min(0)]
    private int _chiitoitsuWeight;

    [SerializeField]
    [Min(0)]
    private int _kokushiWeight;

    [SerializeField]
    [Min(0)]
    private int _churenpotoWeight;
    
    public string Id => _id;
    
    public string DisplayName => _displayName;

    public int NumberOfSuits => _numberOfSuits;

    public int NoHonorsWeight => _noHonorsWeight;

    public int WindsWeight => _windsWeight;

    public int DragonsWeight => _dragonsWeight;

    public int WindsAndDragonsWeight => _windsAndDragonsWeight;
    
    public int ChiitoitsuWeight => _chiitoitsuWeight;

    public int KokushiWeight => _kokushiWeight;

    public int ChurenpotoWeight =>
        _churenpotoWeight;
    
    public HandGenerationSettings GetGenerationSettings()
    {
        return new HandGenerationSettings(
            _numberOfSuits,
            _noHonorsWeight,
            _windsWeight,
            _dragonsWeight,
            _windsAndDragonsWeight,
            _chiitoitsuWeight,
            _kokushiWeight,
            _churenpotoWeight);
    }
}