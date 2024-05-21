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

        private void OnSecondsTicked()
        {
            _secondsToAddEnergy--;

            int minutes = _secondsToAddEnergy / 60;
            int seconds = _secondsToAddEnergy % 60;

            foreach (EnergyView view in _views)
            {
                view.UpdateTimer(minutes, seconds);
            }
        }

        private void OnValueChanged(int newValue)
        {
            float scrollValue = (float)newValue / _energySettings.MaxEnergyCount;
            
            foreach (EnergyView view in _views)
            {
                view.UpdateEnergyValue(newValue, _energySettings.MaxEnergyCount, scrollValue);
            }
            
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

        private void ShowOrHideTimers(bool active)
        {
            foreach (EnergyView view in _views)
            {
                view.Timer.SetActive(active);
            }
        }
    }
}