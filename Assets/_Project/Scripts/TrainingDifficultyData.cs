using UnityEngine;

[CreateAssetMenu(
    fileName = "TrainingDifficulty",
    menuName = "Riichi Dojo/Training Difficulty")]
public class TrainingDifficultyData : ScriptableObject
{
    [SerializeField]
    private string _displayName;

    [SerializeField]
    private int _minWaits;

    [SerializeField]
    private int _maxWaits;

    public string DisplayName => _displayName;

    public int MinWaits => _minWaits;

    public int MaxWaits => _maxWaits;
}