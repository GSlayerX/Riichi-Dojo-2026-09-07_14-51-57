using System;

namespace RiichiDojo
{
    [Serializable]
    public class Statistics
    {
        public StatisticsData OneSuit = new();
        public StatisticsData TwoSuits = new();
        public StatisticsData ThreeSuits = new();

        public int QuestionsAnswered =>
            OneSuit.QuestionsAnswered +
            TwoSuits.QuestionsAnswered +
            ThreeSuits.QuestionsAnswered;

        public int CorrectAnswers =>
            OneSuit.CorrectAnswers +
            TwoSuits.CorrectAnswers +
            ThreeSuits.CorrectAnswers;

        public float Accuracy =>
            QuestionsAnswered == 0
                ? 0f
                : (float)CorrectAnswers / QuestionsAnswered * 100f;

        public StatisticsData GetByDifficultyId(
            string difficultyId)
        {
            switch (difficultyId)
            {
                case "1Suit":
                    return OneSuit;

                case "2Suits":
                    return TwoSuits;

                case "3Suits":
                    return ThreeSuits;

                default:
                    throw new ArgumentException(
                        $"Unknown difficulty ID: {difficultyId}");
            }
        }
        
        public void Reset()
        {
            OneSuit = new StatisticsData();
            TwoSuits = new StatisticsData();
            ThreeSuits = new StatisticsData();
        }
    }
}