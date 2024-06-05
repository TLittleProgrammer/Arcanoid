using System.Collections.Generic;
using App.Scripts.External.GameStateMachine;
using App.Scripts.General.Infrastructure;
using App.Scripts.General.LoadingScreen;
using App.Scripts.General.Popup;
using App.Scripts.Scenes.GameScene.Features.Dotween;
using App.Scripts.Scenes.GameScene.Features.Levels.General.Animations;
using App.Scripts.Scenes.GameScene.Features.Levels.General.View;
using App.Scripts.Scenes.GameScene.Features.Restart;
using App.Scripts.Scenes.GameScene.Features.Time;
using App.Scripts.Scenes.GameScene.States.Gameloop;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Scenes.GameScene.States
{
    public class LoadNextLevelState : IState
    {
        private readonly ILoadingScreen _loadingScreen;
        private readonly IStateMachine _gameStateMachine;
        private readonly IPopupService _popupService;
        private readonly ITimeProvider _timeProvider;
        private readonly IShowLevelAnimation _showLevelAnimation;
        private readonly ILevelViewUpdater _levelViewUpdater;
        private readonly List<IGeneralRestartable> _generalRestartables;
        private readonly List<ICurrentLevelRestartable> _currentLevelRestartables;
        private readonly IUpdateServiceForNewLevel _updateServiceForNewLevel;

        public LoadNextLevelState(
            ILoadingScreen loadingScreen,
            IStateMachine gameStateMachine,
            IPopupService popupService,
            ITimeProvider timeProvider,
            IShowLevelAnimation showLevelAnimation,
            List<IGeneralRestartable> generalRestartables,
            List<ICurrentLevelRestartable> currentLevelRestartables,
            IUpdateServiceForNewLevel updateServiceForNewLevel)
        {
            _loadingScreen = loadingScreen;
            _gameStateMachine = gameStateMachine;
            _popupService = popupService;
            _timeProvider = timeProvider;
            _showLevelAnimation = showLevelAnimation;
            _generalRestartables = generalRestartables;
            _currentLevelRestartables = currentLevelRestartables;
            _updateServiceForNewLevel = updateServiceForNewLevel;
        }

        public async UniTask Enter()
        {
            await _loadingScreen.Show(false);
            
            RestartAll();
            
            await ClosePopups();
            await _updateServiceForNewLevel.Update();
            await _loadingScreen.Hide();
            await _showLevelAnimation.Show();
            
            _gameStateMachine.Enter<GameLoopState>().Forget();
        }

        private async UniTask ClosePopups()
        {
            await _popupService.CloseAll();
            _timeProvider.TimeScale = 1f;
        }

        private void RestartAll()
        {
            foreach (IGeneralRestartable restartable in _generalRestartables)
            {
                restartable.Restart();
            }

            foreach (ICurrentLevelRestartable restartable in _currentLevelRestartables)
            {
                restartable.Restart();
            }
        }

        public async UniTask Exit()
        {
            await UniTask.CompletedTask;
        }
    }
}