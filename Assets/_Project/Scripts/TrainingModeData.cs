using UnityEngine;
using RiichiDojo.Mahjong;

[CreateAssetMenu(
    fileName = "TrainingMode",
    menuName = "Riichi Dojo/Training Mode")]
public class TrainingModeData : ScriptableObject
{
    [SerializeField]
    private string _displayName;

    [SerializeField]
    private bool _isAll;

    [SerializeField]
    private HandPattern _pattern;

    public string DisplayName => _displayName;

    public bool IsAll => _isAll;

    public HandPattern Pattern => _pattern;
}