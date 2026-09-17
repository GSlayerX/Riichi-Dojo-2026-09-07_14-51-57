using UnityEngine;

namespace RiichiDojo
{
    public class StatisticsPanelController : MonoBehaviour
    {
        [SerializeField]
        private StatisticsUI _statisticsUI;

        [SerializeField]
        private GameObject _resetConfirmationPanel;

        private readonly UserSettingsService _userSettingsService = new();

        private void Awake()
        {
            _resetConfirmationPanel.SetActive(false);
        }

        public void Open()
        {
            _statisticsUI.Refresh();
            gameObject.SetActive(true);
        }

        public void Close()
        {
            _resetConfirmationPanel.SetActive(false);
            gameObject.SetActive(false);
        }

        public void ShowResetConfirmation()
        {
            _resetConfirmationPanel.SetActive(true);
        }

        public void CancelReset()
        {
            _resetConfirmationPanel.SetActive(false);
        }

        public void ResetStatistics()
        {
            UserSettings settings =
                _userSettingsService.Load();

            settings.Statistics.Reset();

            _userSettingsService.Save(settings);

            _statisticsUI.Refresh();

            _resetConfirmationPanel.SetActive(false);
        }
    }
}