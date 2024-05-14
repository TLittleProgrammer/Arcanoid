using App.Scripts.General.Popup;
using App.Scripts.General.RootUI;
using App.Scripts.Scenes.GameScene.Settings;
using App.Scripts.Scenes.GameScene.Time;

namespace App.Scripts.Scenes.GameScene.LevelProgress
{
    public class StopGameService : IStopGameService
    {
        private readonly ITimeScaleAnimator _timeScaleAnimator;
        private readonly IPopupService _popupService;
        private readonly RootUIViewProvider _rootUIViewProvider;
        private readonly ILevelProgressService _levelProgressService;

        public StopGameService(
            ITimeScaleAnimator timeScaleAnimator,
            IPopupService popupService,
            RootUIViewProvider rootUIViewProvider,
            ILevelProgressService levelProgressService)
        {
            _timeScaleAnimator = timeScaleAnimator;
            _popupService = popupService;
            _rootUIViewProvider = rootUIViewProvider;
            _levelProgressService = levelProgressService;

            _levelProgressService.LevelPassed += Stop;
        }
        
        public async void Stop()
        {
            _levelProgressService.LevelPassed -= Stop;
            await _timeScaleAnimator.Animate(0f);
            _popupService.Show(PopupTypeId.Win, _rootUIViewProvider.PopupUpViewProvider);
        }

        

        public void Restart()
        {
            _levelProgressService.LevelPassed += Stop;
        }
    }
}