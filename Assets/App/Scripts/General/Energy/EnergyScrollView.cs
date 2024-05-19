using App.Scripts.General.ProjectInitialization.Settings;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace App.Scripts.General.Energy
{
    public class EnergyScrollView : MonoBehaviour, IEnergyScrollView
    {
        [SerializeField] private TMP_Text _currentEnergyText;
        [SerializeField] private TMP_Text _timeToGetEnergy;
        [SerializeField] private Scrollbar _scrollbar;

        private string _energyCountTemplate = "{0}/{1}";
        private string _countToAddEnergyTemplate    = "{0}:{1}";
        private EnergySettings _energySettings;

        public TMP_Text CurrentEnergy => _currentEnergyText;
        public TMP_Text TimeToGetEnergy => _timeToGetEnergy;
        public Scrollbar Scrollbar => _scrollbar;

        [Inject]
        private void Construct(EnergySettings energySettings)
        {
            _energySettings = energySettings;
        }

        public void Initialize(int currentEnergyCounter, int minutes, int seconds)
        {
            SetEnergyText(currentEnergyCounter);
            SetTimeToGetEnergy(minutes, seconds);
        }
        
        public void SetEnergyText(int current)
        {
            _currentEnergyText.text = string.Format(_energyCountTemplate, current.ToString(), _energySettings.MaxEnergyCounter.ToString());
            _scrollbar.size = Mathf.Min(1f, (float)current / _energySettings.MaxEnergyCounter);
        }

        public void SetTimeToGetEnergy(int minutes, int seconds)
        {
            string secondsStr = seconds >= 10 ? seconds.ToString() : $"0{seconds}";
            _timeToGetEnergy.text = string.Format(_countToAddEnergyTemplate, minutes.ToString(), secondsStr);
        }
    }
}