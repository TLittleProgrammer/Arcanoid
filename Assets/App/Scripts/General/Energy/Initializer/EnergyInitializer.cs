using System;
using System.Timers;
using App.Scripts.External.UserData;
using App.Scripts.General.DateTime;
using App.Scripts.General.MVVM.Energy;
using App.Scripts.General.UserData.Energy;
using App.Scripts.General.UserData.Global;
using UnityEngine;

namespace App.Scripts.General.Energy
{
    public sealed class EnergyInitializer : IEnergyInitializer
    {
        private readonly EnergySettings _energySettings;
        private readonly IDateTimeService _dateTimeService;
        private readonly EnergyModel _energyModel;
        private readonly IEnergyDataService _energyDataService;
        private readonly IDataProvider<GlobalData> _dataProvider;
        private readonly GlobalData _globalData;

        public EnergyInitializer(
            EnergySettings energySettings,
            IDateTimeService dateTimeService,
            EnergyModel energyModel,
            IEnergyDataService energyDataService,
            IDataProvider<GlobalData> dataProvider)
        {
            _energySettings = energySettings;
            _dateTimeService = dateTimeService;
            _energyModel = energyModel;
            _energyDataService = energyDataService;
            _dataProvider = dataProvider;
            _globalData = _dataProvider.GetData();
        }


        public void Initialize()
        {
            if (!_globalData.IsFirstEnter)
            {
                int timeDifferences = (int)(_dateTimeService.GetCurrentTimestamp() - _globalData.LastTimestampEnergyWasAdded);

                Debug.Log($"CurrentTime: {_dateTimeService.GetCurrentTimestamp()}; lastTime: {_globalData.LastTimestampEnergyWasAdded}");
                
                UpdateEnergyTimer(timeDifferences);

                if (timeDifferences >= _energySettings.SecondsToRecoveryEnergy)
                {
                    float energyDivided = timeDifferences / _energySettings.SecondsToRecoveryEnergy;
                    int needAddEnergy = (int)Math.Floor(energyDivided);
                    AddEnergy(needAddEnergy);
                    
                    _globalData.LastTimestampEnergyWasAdded = _dateTimeService.GetCurrentTimestamp();
                    _dataProvider.SaveData();
                }
            }
            else
            {
                _energyDataService.Add(_energySettings.InitialEnergyCount);
            }
        }

        private void AddEnergy(int needAddEnergy)
        {
            if (needAddEnergy + _energyDataService.CurrentValue >= _energySettings.MaxEnergyCount)
            {
                if (_energyDataService.CurrentValue < _energySettings.MaxEnergyCount)
                {
                    _energyDataService.Add(_energySettings.MaxEnergyCount - _energyDataService.CurrentValue);
                    
                }
            }
            else
            {
                _energyDataService.Add(needAddEnergy);
            }
        }

        private void UpdateEnergyTimer(int timeDifferences)
        {
            if (timeDifferences < _energySettings.SecondsToRecoveryEnergy)
            {
                _energyModel.SetRemainingSeconds(_energySettings.SecondsToRecoveryEnergy - timeDifferences);
            }
            else
            {
                _energyModel.SetRemainingSeconds(_energySettings.SecondsToRecoveryEnergy - (timeDifferences % _energySettings.SecondsToRecoveryEnergy));
            }
        }
    }
}