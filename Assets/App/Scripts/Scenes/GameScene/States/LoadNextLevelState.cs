using System.Collections.Generic;
using App.Scripts.External.GameStateMachine;
using App.Scripts.General.Infrastructure;
using App.Scripts.General.LoadingScreen;
using App.Scripts.General.Popup;
using App.Scripts.Scenes.GameScene.Features.Entities.Bird.Interfaces;
using App.Scripts.Scenes.GameScene.Features.Levels.General;
using App.Scripts.Scenes.GameScene.Features.Levels.General.Animations;
using App.Scripts.Scenes.GameScene.Features.Levels.General.View;
using App.Scripts.Scenes.GameScene.Features.Levels.Loading;
using App.Scripts.Scenes.GameScene.Features.Levels.Loading.Loader;
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
        private readonly IShowLevelService _showLevelService;
        private readonly ILevelViewUpdater _levelViewUpdater;
        private readonly List<IRestartable> _generalRestartables;
        private readonly IUpdateServiceForNewLevel _updateServiceForNewLevel;
        private readonly IBirdsService _birdsService;

        public LoadNextLevelState(
            ILoadingScreen loadingScreen,
            IStateMachine gameStateMachine,
            IPopupService popupService,
            ITimeProvider timeProvider,
            IShowLevelService showLevelService,
            List<IRestartable> generalRestartables,
            IUpdateServiceForNewLevel updateServiceForNewLevel,
            IBirdsService birdsService)
        {
            _loadingScreen = loadingScreen;
            _gameStateMachine = gameStateMachine;
            _popupService = popupService;
            _timeProvider = timeProvider;
            _showLevelService = showLevelService;
            _generalRestartables = generalRestartables;
            _updateServiceForNewLevel = updateServiceForNewLevel;
            _birdsService = birdsService;
        }

        public async UniTask Enter()
        {
            await _loadingScreen.Show(false);
            
            RestartAll();
            _birdsService.StopAll();

            await ClosePopups();
            await _updateServiceForNewLevel.Update();
            await _loadingScreen.Hide();
            await _showLevelService.Show();
            
            _gameStateMachine.Enter<GameLoopState>().Forget();
        }

        private async UniTask ClosePopups()
        {
            await _popupService.CloseAll();
            _timeProvider.TimeScale = 1f;
        }

        private void RestartAll()
        {
            foreach (IRestartable restartable in _generalRestartables)
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