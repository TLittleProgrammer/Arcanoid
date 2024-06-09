using System;
using App.Scripts.General.Energy;
using App.Scripts.General.Reactive;
using App.Scripts.General.Time;
using App.Scripts.General.UserData.Energy;
using UnityEngine;
using Zenject;

namespace App.Scripts.General.MVVM.Energy
{
    public class EnergyModel : IInitializable
    {
        public readonly ReactiveProperty<int> SecondsToAddEnergy = new();
        public readonly ReactiveProperty<int> CurrentEnergy = new();
        
        private readonly ITimeTicker _timeTicker;
        private readonly EnergySettings _energySettings;
        private readonly IEnergyDataService _energyDataService;

        private bool _isTickes;

        public int MaxEnergy => _energySettings.MaxEnergyCount;

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
            
            _energyDataService.ValueChanged += OnEnergyValueChanged;
            CurrentEnergy.Value = _energyDataService.CurrentValue;
            SecondsToAddEnergy.Value = _energySettings.SecondsToRecoveryEnergy;
        }

        public void SetRemainingSeconds(int seconds)
        {
            SecondsToAddEnergy.Value = seconds;
        }

        private void OnSecondsTicked()
        {
            SecondsToAddEnergy.Value--;
            
            if (SecondsToAddEnergy.Value <= 0)
            {
                _isTickes = false;
                _timeTicker.SecondsTicked -= OnSecondsTicked;
                _energyDataService.Add(1);
            }
        }

        private void OnEnergyValueChanged(int newValue)
        {
            CurrentEnergy.Value = newValue;
            
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
                
                SecondsToAddEnergy.Value = _energySettings.SecondsToRecoveryEnergy;
            }
        }
    }
}