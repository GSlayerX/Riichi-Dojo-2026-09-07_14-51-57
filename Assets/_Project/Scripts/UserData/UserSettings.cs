using System;

namespace RiichiDojo
{
    [Serializable]
    public class UserSettings
    {
        public int Version = 2;
        
        public string TrainingDifficulty = "1Suit";

        public string TileTheme = "White";
    }
}