using System;
using App.Scripts.External.UserData;
using App.Scripts.General.DateTime;
using App.Scripts.General.Energy;
using App.Scripts.General.ProjectInitialization.Settings;
using App.Scripts.General.UserData.Energy;
using App.Scripts.General.UserData.Persistent;
using UnityEngine;
using Zenject;

namespace App.Scripts.General.ProjectInitialization.Installers
{
    public class ProjectInitializer : IInitializable
    {
        private readonly ApplicationSettings _applicationSettings;
        private readonly IUserDataContainer _userDataContainer;
        private readonly IDateTimeService _dateTimeService;
        private readonly EnergySettings _energySettings;
        private readonly IEnergyDataService _energyDataService;
        private readonly EnergyService _energyService;

        public ProjectInitializer(
            ApplicationSettings applicationSettings,
            IUserDataContainer userDataContainer,
            IDateTimeService dateTimeService,
            EnergySettings energySettings,
            IEnergyDataService energyDataService,
            EnergyService energyService)
        {
            _applicationSettings = applicationSettings;
            _userDataContainer = userDataContainer;
            _dateTimeService = dateTimeService;
            _energySettings = energySettings;
            _energyDataService = energyDataService;
            _energyService = energyService;
        }
        
        public async void Initialize()
        {
            Application.targetFrameRate = _applicationSettings.TargetFPS;
            QualitySettings.vSyncCount = _applicationSettings.VSyncCounter;

            InitializeUserData();

            await _energyService.AsyncInitialize();
        }

        private void InitializeUserData()
        {
            PersistentData persistentData = (PersistentData)_userDataContainer.GetData<PersistentData>();

            if (persistentData.IsFirstEnter)
            {
                persistentData.IsFirstEnter = false;
            }
            else
            {
                InitializeEnergy(persistentData);
            }
            
            persistentData.LastVisit = _dateTimeService.GetCurrentTimestamp();
                
            _userDataContainer.SaveData<PersistentData>();
        }

        private void InitializeEnergy(PersistentData persistentData)
        {
            int currentEnergyCounter = _energyDataService.GetEnergy();

            if (currentEnergyCounter >= _energySettings.MaxEnergyCounter)
                return;
            
            long secondsFromLastVisit = _dateTimeService.GetCurrentTimestamp() - persistentData.LastVisit;
            long offsetBetweenCurrentAndMaxEnergyCounter = _energySettings.MaxEnergyCounter - _energyDataService.GetEnergy();
                
            long addEnergyCounter = secondsFromLastVisit / _energySettings.SecondsToAddOneEnergy;
                
            _energyDataService.AddEnergy((int)Math.Min(addEnergyCounter, offsetBetweenCurrentAndMaxEnergyCounter));
        }
    }
}