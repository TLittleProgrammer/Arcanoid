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
        private readonly IConverter _converter;
        private readonly TextAsset _localisation;
        private readonly EnergyModel _energyModel;
        private readonly IEnergyDataService _energyDataService;
        private readonly IDataProvider<GlobalData> _globalDataProvider;
        private readonly EnergySettings _energySettings;
        private readonly IDateTimeService _dateTimeService;
        private readonly IGlobalDataService _globalDataService;

        public ProjectInitializer(
            ApplicationSettings applicationSettings,
            ILocaleService localeService,
            IConverter converter,
            TextAsset localisation,
            EnergyModel energyModel,
            IEnergyDataService energyDataService,
            IDataProvider<GlobalData> globalDataProvider,
            EnergySettings energySettings,
            IDateTimeService dateTimeService,
            IGlobalDataService globalDataService)
        {
            _applicationSettings = applicationSettings;
            _localeService = localeService;
            _converter = converter;
            _localisation = localisation;
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
            
            InitializeLocale();
            
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
                        _energyDataService.Add(Mathf.Abs(_energySettings.MaxEnergyCount - _energyDataService.CurrentValue - needAddEnergy));
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

        private void InitializeLocale()
        {
            string[,] csvGrid = _converter.ConvertFileToGrid(_localisation.text);

            Dictionary<string, Dictionary<string, string>> localeStorage = new();

            InitializeLocaleStorage(csvGrid, localeStorage);
            FillStorage(csvGrid, ref localeStorage);
            
            _localeService.SetStorage(localeStorage);
        }

        private static void InitializeLocaleStorage(string[,] csvGrid, Dictionary<string, Dictionary<string, string>> localeStorage)
        {
            for (int x = 1; x < csvGrid.GetLength(0); x++)
            {
                string languageKey = csvGrid[x, 0];

                if (!string.IsNullOrEmpty(languageKey))
                {
                    localeStorage.Add(languageKey, new Dictionary<string, string>());
                }
            }
        }

        private void FillStorage(string[,] csvGrid, ref Dictionary<string, Dictionary<string, string>> localeStorage)
        {
            for (int i = 1; i < csvGrid.GetLength(0); i++)
            {
                string languageKey = csvGrid[i, 0];

                if (string.IsNullOrEmpty(languageKey))
                    continue;

                for (int j = 1; j < csvGrid.GetLength(1) - 1; j++)
                {
                    if (csvGrid[0, j] is not null)
                    {
                        localeStorage[languageKey].Add(csvGrid[0, j], csvGrid[i, j]);
                    }
                }
            }
        }
    }
}