using System;
using App.Scripts.General.Energy;
using App.Scripts.General.Time;
using App.Scripts.General.UserData.Energy;
using Zenject;

namespace App.Scripts.General.MVVM.Energy
{
    public class EnergyModel : IInitializable
    {
        private readonly ITimeTicker _timeTicker;
        private readonly EnergySettings _energySettings;
        private readonly IEnergyDataService _energyDataService;

        private bool _isTickes;
        private int _secondsToAddEnergy;

        public event Action<int> RemainingSeconds;
        public event Action<int, int> ValueChanged;

        public int SecondsRemaining => _secondsToAddEnergy;
        public int MaxEnergy => _energySettings.MaxEnergyCount;
        public int CurrentEnergy => _energyDataService.CurrentValue;

        public EnergyModel(
            ITimeTicker timeTicker,
            EnergySettings energySettings,
            IEnergyDataService energyDataService)
        {
            _timeTicker = timeTicker;
            _energySettings = energySettings;
            _energyDataService = energyDataService;
        }

        public void Initialize()
        {
            if (_isTickes == false && _energyDataService.CurrentValue < _energySettings.MaxEnergyCount)
            {
                _isTickes = true;
                _timeTicker.SecondsTicked += OnSecondsTicked;
            }
            
            _energyDataService.ValueChanged += OnValueChanged;
            _secondsToAddEnergy = _energySettings.SecondsToRecoveryEnergy;
        }

        public void SetRemainingSeconds(int seconds)
        {
            _secondsToAddEnergy = seconds;
        }

        private void OnSecondsTicked()
        {
            _secondsToAddEnergy--;
            
            RemainingSeconds?.Invoke(_secondsToAddEnergy);
            
            if (_secondsToAddEnergy <= 0)
            {
                _isTickes = false;
                _timeTicker.SecondsTicked -= OnSecondsTicked;
                _energyDataService.Add(1);
            }
        }

        private void OnValueChanged(int newValue)
        {
            ValueChanged?.Invoke(newValue, _energySettings.MaxEnergyCount);
            
            if (newValue >= _energySettings.MaxEnergyCount)
            {
                _timeTicker.SecondsTicked -= OnSecondsTicked;
            }
            else
            {
                if (_isTickes == false)
                {
                    _isTickes = true;
                    _timeTicker.SecondsTicked += OnSecondsTicked;
                } 
                
                _secondsToAddEnergy = _energySettings.SecondsToRecoveryEnergy;
            }
        }
    }
}