using System;
using System.Threading.Tasks;
using App.Scripts.External.Initialization;
using App.Scripts.External.UserData;
using App.Scripts.General.DateTime;
using App.Scripts.General.ProjectInitialization.Settings;
using App.Scripts.General.Ticker;
using App.Scripts.General.UserData.Energy;
using App.Scripts.General.UserData.Persistent;
using Cysharp.Threading.Tasks;

namespace App.Scripts.General.Energy
{
    public class EnergyService : IAsyncInitializable
    {
        private readonly IUserDataContainer _userDataContainer;
        private readonly ITimeTicker _timeTicker;
        private readonly IDateTimeService _dateTimeService;
        private readonly EnergySettings _energySettings;
        private readonly IEnergyDataService _energyDataService;

        private float _secondsToAddEnergy;
        private EnergyData _energyData;
        private bool _isRecovery = false;
        private EnergyScrollView _energyScrollView;

        public EnergyService(
            IUserDataContainer userDataContainer,
            ITimeTicker timeTicker,
            IDateTimeService dateTimeService,
            EnergySettings energySettings,
            IEnergyDataService energyDataService)
        {
            _userDataContainer = userDataContainer;
            _timeTicker = timeTicker;
            _dateTimeService = dateTimeService;
            _energySettings = energySettings;
            _energyDataService = energyDataService;

            _energyDataService.EnergyWasChanged += OnEnergyWasChanged;
        }

        public async UniTask AsyncInitialize()
        {
            PersistentData persistentData = (PersistentData)_userDataContainer.GetData<PersistentData>();

            _energyData = (EnergyData)_userDataContainer.GetData<EnergyData>();
            long timeOffset = _dateTimeService.GetCurrentTimestamp() - persistentData.LastVisit;

            //_secondsToAddEnergy = Math.Min(timeOffset, _energySettings.SecondsToAddOneEnergy);
            
            await UniTask.CompletedTask;
        }

        public void SetView(EnergyScrollView energyScrollView)
        {
            _energyScrollView = energyScrollView;
            _secondsToAddEnergy = _energySettings.SecondsToAddOneEnergy;
            
            energyScrollView.Initialize(_energyData.EnergyCounter, 1, 1);

            if (_energyData.EnergyCounter >= _energySettings.MaxEnergyCounter)
            {
                _energyScrollView.TimeToGetEnergy.gameObject.SetActive(false);
            }
        }

        private void OnSecondsTicked()
        {
            if (_energyScrollView is null)
                return;
            
            _secondsToAddEnergy--;

            int minutes = (int)_secondsToAddEnergy / 60;
            int seconds = (int)_secondsToAddEnergy % 60;

            _energyScrollView.SetTimeToGetEnergy(minutes, seconds);

            if (_secondsToAddEnergy == 0)
            {
                _isRecovery = false;
                _energyScrollView.TimeToGetEnergy.gameObject.SetActive(false);
                _timeTicker.SecondsTicked -= OnSecondsTicked;
                _energyDataService.AddEnergy(1);
            }
        }

        private void OnEnergyWasChanged(int energy)
        {
            if (_energyScrollView is not null)
            {
                _energyScrollView.SetEnergyText(energy);
            }
            
            if (energy >= _energySettings.MaxEnergyCounter)
            {
                _isRecovery = false;
                _timeTicker.SecondsTicked -= OnSecondsTicked;
            }
            else
            {
                if (_isRecovery is false)
                {
                    if (_energyScrollView is not null)
                    {
                        _energyScrollView.TimeToGetEnergy.gameObject.SetActive(true);
                    }
                    
                    _secondsToAddEnergy = _energySettings.SecondsToAddOneEnergy;
                    _isRecovery = true;
                    _timeTicker.SecondsTicked += OnSecondsTicked;
                }
            }
        }
    }
}