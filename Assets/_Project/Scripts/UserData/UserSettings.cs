using System;

namespace RiichiDojo
{
    [Serializable]
    public class UserSettings
    {
        public int Version = 3;
        
        public string TrainingDifficulty = "1Suit";

        public string TileTheme = "White";
        
        public Statistics Statistics = new();
    }
}