using App.Scripts.General.Popup;
using App.Scripts.General.RootUI;
using App.Scripts.Scenes.GameScene.Settings;
using App.Scripts.Scenes.GameScene.Time;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.LevelProgress
{
    public class StopGameService : IStopGameService
    {
        private readonly ITimeProvider _timeProvider;
        private readonly StopGameSettings _stopGameSettings;
        private readonly IPopupService _popupService;
        private readonly RootUIViewProvider _rootUIViewProvider;
        private readonly ILevelProgressService _levelProgressService;

        public StopGameService(
            ITimeProvider timeProvider,
            StopGameSettings stopGameSettings,
            IPopupService popupService,
            RootUIViewProvider rootUIViewProvider,
            ILevelProgressService levelProgressService)
        {
            _timeProvider = timeProvider;
            _stopGameSettings = stopGameSettings;
            _popupService = popupService;
            _rootUIViewProvider = rootUIViewProvider;
            _levelProgressService = levelProgressService;

            _levelProgressService.LevelPassed += Stop;
        }
        
        public async void Stop()
        {
            _levelProgressService.LevelPassed -= Stop;
            await AnimateTimeScale();
            _popupService.Show(PopupTypeId.Win, _rootUIViewProvider.PopupUpViewProvider);
        }

        private UniTask AnimateTimeScale()
        {
            return DOVirtual.Float(1f, 0f, _stopGameSettings.TimeScaleChangeDuration, UpdateTimeScale).ToUniTask();
        }

        private void UpdateTimeScale(float value)
        {
            _timeProvider.TimeScale = value;
        }

        public void Restart()
        {
            _levelProgressService.LevelPassed += Stop;
        }
    }
}