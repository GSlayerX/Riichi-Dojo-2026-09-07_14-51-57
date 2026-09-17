using System;

namespace RiichiDojo
{
    [Serializable]
    public class StatisticsData
    {
        public int QuestionsAnswered;
        public int CorrectAnswers;

        public float Accuracy =>
            QuestionsAnswered == 0
                ? 0f
                : (float)CorrectAnswers / QuestionsAnswered * 100f;

        public void RecordAnswer(bool correct)
        {
            QuestionsAnswered++;

            if (correct)
            {
                CorrectAnswers++;
            }
        }
    }
}