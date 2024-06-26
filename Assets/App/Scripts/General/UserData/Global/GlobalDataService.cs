﻿using App.Scripts.External.UserData;
using App.Scripts.General.DateTime;
using App.Scripts.General.MVVM.Energy;
using Cysharp.Threading.Tasks;

namespace App.Scripts.General.UserData.Global
{
    public sealed class GlobalDataService : IGlobalDataService
    {
        private readonly IDataProvider<GlobalData> _globalDataProvider;
        private readonly IDateTimeService _dateTimeService;
        private readonly EnergyModel _energyModel;

        private GlobalData _globalData;
        
        public GlobalDataService(
            IDataProvider<GlobalData> globalDataProvider,
            IDateTimeService dateTimeService,
            EnergyModel energyModel)
        {
            _globalDataProvider = globalDataProvider;
            _dateTimeService = dateTimeService;
            _energyModel = energyModel;
        }

        public async UniTask AsyncInitialize()
        {
            _globalData = _globalDataProvider.GetData();

            _energyModel.SecondsToAddEnergy.OnChanged += OnSecondsToAddEnergyWasChanged;
            
            await UniTask.CompletedTask;
        }

        private void OnSecondsToAddEnergyWasChanged(int seconds)
        {
            if (seconds <= 0)
            {
                _globalData.LastTimestampEnergyWasAdded = _dateTimeService.GetCurrentTimestamp();
                _globalDataProvider.SaveData();
            }
        }
    }
}