using App.Scripts.External.UserData;
using App.Scripts.General.DateTime;
using App.Scripts.General.ProjectInitialization.Settings;
using App.Scripts.General.Time;
using Cysharp.Threading.Tasks;

namespace App.Scripts.General.UserData.Global
{
    public sealed class GlobalDataService : IGlobalDataService
    {
        private readonly ApplicationSettings _applicationSettings;
        private readonly ITimeTicker _ticker;
        private readonly IUserDataContainer _userDataContainer;
        private readonly IDateTimeService _dateTimeService;
        
        private int _passedSeconds = 0;
        private GlobalData _globalData;
        
        public GlobalDataService(
            ApplicationSettings applicationSettings,
            ITimeTicker ticker,
            IUserDataContainer userDataContainer,
            IDateTimeService dateTimeService)
        {
            _applicationSettings = applicationSettings;
            _ticker = ticker;
            _userDataContainer = userDataContainer;
            _dateTimeService = dateTimeService;
        }

        public async UniTask AsyncInitialize()
        {
            _globalData = (GlobalData)_userDataContainer.GetData<GlobalData>();
            _ticker.SecondsTicked += OnSecondsTicked;
            
            await UniTask.CompletedTask;
        }

        private void OnSecondsTicked()
        {
            _passedSeconds++;

            if (_passedSeconds >= _applicationSettings.SaveGlobalDataEnterFromSeconds)
            {
                _globalData.LastTimestampEnter = _dateTimeService.GetCurrentTimestamp();
                _passedSeconds = 0;
                
                _userDataContainer.SaveData<GlobalData>();
            }
        }
    }
}