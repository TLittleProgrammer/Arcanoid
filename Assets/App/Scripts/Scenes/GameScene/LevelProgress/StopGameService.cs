using App.Scripts.External.GameStateMachine;
using App.Scripts.General.Popup;
using App.Scripts.General.RootUI;
using App.Scripts.Scenes.GameScene.Settings;
using App.Scripts.Scenes.GameScene.States;
using App.Scripts.Scenes.GameScene.Time;

namespace App.Scripts.Scenes.GameScene.LevelProgress
{
    public class StopGameService : IStopGameService
    {
        private readonly IStateMachine _stateMachine;
        private readonly ITimeScaleAnimator _timeScaleAnimator;
        private readonly IPopupService _popupService;
        private readonly RootUIViewProvider _rootUIViewProvider;
        private readonly ILevelProgressService _levelProgressService;

        public StopGameService(IStateMachine stateMachine, ILevelProgressService levelProgressService)
        {
            _levelProgressService = levelProgressService;
            _stateMachine = stateMachine;

            _levelProgressService.LevelPassed += Stop;
        }
        
        public void Stop()
        {
            _stateMachine.Enter<WinState>();
            _levelProgressService.LevelPassed -= Stop;
        }

        public void Restart()
        {
            _levelProgressService.LevelPassed += Stop;
        }
    }
}