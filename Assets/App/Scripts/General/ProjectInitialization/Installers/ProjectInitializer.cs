using System.Collections.Generic;
using App.Scripts.External.Localisation;
using App.Scripts.External.Localisation.Converters;
using App.Scripts.External.UserData;
using App.Scripts.General.DateTime;
using App.Scripts.General.Energy;
using App.Scripts.General.MVVM.Energy;
using App.Scripts.General.ProjectInitialization.Settings;
using App.Scripts.General.UserData.Energy;
using App.Scripts.General.UserData.Global;
using UnityEngine;
using Zenject;

namespace App.Scripts.General.ProjectInitialization.Installers
{
    public class ProjectInitializer : IInitializable
    {
        private readonly ApplicationSettings _applicationSettings;
        private readonly ILocaleService _localeService;
        private readonly EnergyModel _energyModel;
        private readonly IEnergyDataService _energyDataService;
        private readonly IDataProvider<GlobalData> _globalDataProvider;
        private readonly EnergySettings _energySettings;
        private readonly IDateTimeService _dateTimeService;
        private readonly IGlobalDataService _globalDataService;

        public ProjectInitializer(
            ApplicationSettings applicationSettings,
            ILocaleService localeService,
            EnergyModel energyModel,
            IEnergyDataService energyDataService,
            IDataProvider<GlobalData> globalDataProvider,
            EnergySettings energySettings,
            IDateTimeService dateTimeService,
            IGlobalDataService globalDataService)
        {
            _applicationSettings = applicationSettings;
            _localeService = localeService;
            _energyModel = energyModel;
            _energyDataService = energyDataService;
            _globalDataProvider = globalDataProvider;
            _energySettings = energySettings;
            _dateTimeService = dateTimeService;
            _globalDataService = globalDataService;
        }
        
        public async void Initialize()
        {
            Application.targetFrameRate = _applicationSettings.TargetFPS;
            QualitySettings.vSyncCount = _applicationSettings.VSyncCounter;

            _localeService.SetLocaleKey(LocaleConstants.DefaultLocaleKey);
            
            GlobalData globalData = _globalDataProvider.GetData();
            
            InitializeEnergyData(globalData);
            CheckFirstEnter(globalData);

            await _globalDataService.AsyncInitialize();
        }

        private void CheckFirstEnter(GlobalData globalData)
        {
            if (globalData.IsFirstEnter)
            {
                globalData.IsFirstEnter = false;
                _globalDataProvider.SaveData(globalData);
            }
        }

        private void InitializeEnergyData(GlobalData globalData)
        {
            if (!globalData.IsFirstEnter)
            {
                int needAddEnergy = (int)(_dateTimeService.GetCurrentTimestamp() - globalData.LastTimestampEnter) / _energySettings.SecondsToRecoveryEnergy;

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
                
                _energyModel.SetRemainingSeconds(needAddEnergy % _energySettings.SecondsToRecoveryEnergy);
            }
            else
            {
                _energyDataService.Add(_energySettings.InitialEnergyCount);
            }
        }
    }
}