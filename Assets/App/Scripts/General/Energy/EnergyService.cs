using System.Collections.Generic;
using App.Scripts.General.Time;
using App.Scripts.General.UserData.Energy;
using Cysharp.Threading.Tasks;

namespace App.Scripts.General.Energy
{
    public class EnergyService : IEnergyService
    {
        private readonly ITimeTicker _ticker;
        private readonly EnergySettings _energySettings;
        private readonly IEnergyDataService _energyDataService;

        private List<EnergyView> _views = new();
        private int _secondsToAddEnergy;
        private int _currentMinutes;
        private int _currentSeconds;

        public EnergyService(
            ITimeTicker ticker,
            EnergySettings energySettings,
            IEnergyDataService energyDataService)
        {
            _ticker = ticker;
            _energySettings = energySettings;
            _energyDataService = energyDataService;
        }

        public async UniTask AsyncInitialize()
        {
            _ticker.SecondsTicked += OnSecondsTicked;
            _energyDataService.ValueChanged += OnValueChanged;
            _secondsToAddEnergy = _energySettings.SecondsToRecoveryEnergy;
            
            await UniTask.CompletedTask;
        }

        public void AddView(EnergyView view)
        {
            view.Timer.SetActive(_energyDataService.CurrentValue < _energySettings.MaxEnergyCount);
            view.InitializeView(new()
            {
                Energy    = _energyDataService.CurrentValue,
                Minutes   = _currentMinutes,
                Seconds   = _currentSeconds,
                MaxEnergy = _energySettings.MaxEnergyCount
            });
            
            _views.Add(view);
        }

        public void RemoveView(EnergyView view)
        {
            _views.Remove(view);
        }

        private void OnSecondsTicked()
        {
            _secondsToAddEnergy--;

            _currentMinutes = _secondsToAddEnergy / 60;
            _currentSeconds = _secondsToAddEnergy % 60;

            UpdateTimer(_currentMinutes, _currentSeconds);

            if (_secondsToAddEnergy <= 0)
            {
                _ticker.SecondsTicked -= OnSecondsTicked;
                _energyDataService.Add(1);
            }
        }

        private void OnValueChanged(int newValue)
        {
            UpdateValues(newValue);

            if (newValue >= _energySettings.MaxEnergyCount)
            {
                _ticker.SecondsTicked -= OnSecondsTicked;
                ShowOrHideTimers(false);
            }
            else
            {
                _secondsToAddEnergy = _energySettings.SecondsToRecoveryEnergy;
                _ticker.SecondsTicked += OnSecondsTicked;
                ShowOrHideTimers(true);
            }
        }

        private void UpdateTimer(int minutes, int seconds)
        {
            foreach (EnergyView view in _views)
            {
                view.UpdateTimer(minutes, seconds);
            }
        }

        private void UpdateValues(int newValue)
        {
            float scrollValue = (float)newValue / _energySettings.MaxEnergyCount;

            foreach (EnergyView view in _views)
            {
                view.UpdateEnergyValue(newValue, _energySettings.MaxEnergyCount, scrollValue);
            }
        }

        private void ShowOrHideTimers(bool active)
        {
            foreach (EnergyView view in _views)
            {
                view.Timer.SetActive(active);
            }
        }

        public void Dispose()
        {
            _views.Clear();
        }
    }
}