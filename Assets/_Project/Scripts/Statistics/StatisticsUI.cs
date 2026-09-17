using TMPro;
using UnityEngine;

namespace RiichiDojo
{
    public class StatisticsUI : MonoBehaviour
    {
        [Header("Overall")]

        [SerializeField]
        private TMP_Text _overallQuestionsAnsweredText;

        [SerializeField]
        private TMP_Text _overallCorrectAnswersText;

        [Header("1 Suit")]

        [SerializeField]
        private TMP_Text _oneSuitQuestionsAnsweredText;

        [SerializeField]
        private TMP_Text _oneSuitCorrectAnswersText;

        [Header("2 Suits")]

        [SerializeField]
        private TMP_Text _twoSuitsQuestionsAnsweredText;

        [SerializeField]
        private TMP_Text _twoSuitsCorrectAnswersText;

        [Header("3 Suits")]

        [SerializeField]
        private TMP_Text _threeSuitsQuestionsAnsweredText;

        [SerializeField]
        private TMP_Text _threeSuitsCorrectAnswersText;

        private readonly UserSettingsService _userSettingsService = new();

        private void OnEnable()
        {
            Refresh();
        }

        public void Refresh()
        {
            UserSettings settings =
                _userSettingsService.Load();

            Statistics statistics =
                settings.Statistics;

            SetStatistics(
                statistics,
                _overallQuestionsAnsweredText,
                _overallCorrectAnswersText);

            SetStatistics(
                statistics.OneSuit,
                _oneSuitQuestionsAnsweredText,
                _oneSuitCorrectAnswersText);

            SetStatistics(
                statistics.TwoSuits,
                _twoSuitsQuestionsAnsweredText,
                _twoSuitsCorrectAnswersText);

            SetStatistics(
                statistics.ThreeSuits,
                _threeSuitsQuestionsAnsweredText,
                _threeSuitsCorrectAnswersText);
        }

        private void SetStatistics(
            StatisticsData statistics,
            TMP_Text questionsAnsweredText,
            TMP_Text correctAnswersText)
        {
            if (statistics.QuestionsAnswered == 0)
            {
                questionsAnsweredText.text =
                    "Questions Answered: -";

                correctAnswersText.text =
                    "Correct Answers: -";

                return;
            }

            questionsAnsweredText.text =
                $"Questions Answered: {statistics.QuestionsAnswered}";

            correctAnswersText.text =
                $"Correct Answers: {statistics.CorrectAnswers} " +
                $"({statistics.Accuracy:0.#}%)";
        }

        private void SetStatistics(
            Statistics statistics,
            TMP_Text questionsAnsweredText,
            TMP_Text correctAnswersText)
        {
            if (statistics.QuestionsAnswered == 0)
            {
                questionsAnsweredText.text =
                    "Questions Answered: -";

                correctAnswersText.text =
                    "Correct Answers: -";

                return;
            }

            questionsAnsweredText.text =
                $"Questions Answered: {statistics.QuestionsAnswered}";

            correctAnswersText.text =
                $"Correct Answers: {statistics.CorrectAnswers} " +
                $"({statistics.Accuracy:0.#}%)";
        }
    }
}