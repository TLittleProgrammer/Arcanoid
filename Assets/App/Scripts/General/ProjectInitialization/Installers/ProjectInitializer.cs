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
        private readonly IDataProvider<GlobalData> _globalDataProvider;
        private readonly IGlobalDataService _globalDataService;
        private readonly IEnergyInitializer _energyInitializer;

        public ProjectInitializer(
            ApplicationSettings applicationSettings,
            ILocaleService localeService,
            IDataProvider<GlobalData> globalDataProvider,
            IGlobalDataService globalDataService,
            IEnergyInitializer energyInitializer)
        {
            _applicationSettings = applicationSettings;
            _localeService = localeService;
            _globalDataProvider = globalDataProvider;
            _globalDataService = globalDataService;
            _energyInitializer = energyInitializer;
        }
        
        public async void Initialize()
        {
            Application.targetFrameRate = _applicationSettings.TargetFPS;
            QualitySettings.vSyncCount = _applicationSettings.VSyncCounter;

            _localeService.SetLocaleKey(LocaleConstants.DefaultLocaleKey);
            
            GlobalData globalData = _globalDataProvider.GetData();
            
            _energyInitializer.Initialize();
            CheckFirstEnter(globalData);

            await _globalDataService.AsyncInitialize();
        }

        private void CheckFirstEnter(GlobalData globalData)
        {
            if (globalData.IsFirstEnter)
            {
                globalData.IsFirstEnter = false;
                _globalDataProvider.SaveData();
            }
        }
    }
}