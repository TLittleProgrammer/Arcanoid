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
        private readonly IDataProvider<GlobalData> _globalDataProvider;
        private readonly IDateTimeService _dateTimeService;
        
        private int _passedSeconds = 0;
        private GlobalData _globalData;
        
        public GlobalDataService(
            ApplicationSettings applicationSettings,
            ITimeTicker ticker,
            IDataProvider<GlobalData> globalDataProvider,
            IDateTimeService dateTimeService)
        {
            _applicationSettings = applicationSettings;
            _ticker = ticker;
            _globalDataProvider = globalDataProvider;
            _dateTimeService = dateTimeService;
        }

        public async UniTask AsyncInitialize()
        {
            _globalData = _globalDataProvider.GetData();
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
                
                _globalDataProvider.SaveData(_globalData);
            }
        }
    }
}