using System.Collections.Generic;
using App.Scripts.External.GameStateMachine;
using App.Scripts.General.Infrastructure;
using App.Scripts.General.Levels.LevelPackInfoService;
using App.Scripts.General.LoadingScreen;
using App.Scripts.General.Popup;
using App.Scripts.Scenes.GameScene.Features.Dotween;
using App.Scripts.Scenes.GameScene.Features.Entities.Ball;
using App.Scripts.Scenes.GameScene.Features.Entities.Bird.Interfaces;
using App.Scripts.Scenes.GameScene.Features.Levels.General;
using App.Scripts.Scenes.GameScene.Features.Levels.General.Animations;
using App.Scripts.Scenes.GameScene.Features.Levels.Loading;
using App.Scripts.Scenes.GameScene.States.Gameloop;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;

namespace App.Scripts.Scenes.GameScene.States
{
    public class RestartState : IState
    {
        private readonly ILoadingScreen _loadingScreen;
        private readonly List<ICurrentLevelRestartable> _currentLevelRestartables;
        private readonly IStateMachine _gameStateMachine;
        private readonly ITweenersLocator _tweenersLocator;
        private readonly IShowLevelService _showLevelAnimation;
        private readonly IBallsService _ballsService;
        private readonly List<IGeneralRestartable> _generalRestartables;
        private readonly IPopupService _popupService;
        private readonly IBirdsService _birdsService;
        private readonly ILevelDataChooser _levelDataChooser;
        private readonly ILevelPackInfoService _levelPackInfoService;

        public RestartState(
            ILoadingScreen loadingScreen,
            List<ICurrentLevelRestartable> currentLevelRestartables,
            IStateMachine gameStateMachine,
            ITweenersLocator tweenersLocator,
            IShowLevelService showLevelAnimation,
            IBallsService ballsService,
            List<IGeneralRestartable> generalRestartables,
            IPopupService popupService,
            IBirdsService birdsService,
            ILevelDataChooser levelDataChooser
        )
        {
            _loadingScreen = loadingScreen;
            _currentLevelRestartables = currentLevelRestartables;
            _gameStateMachine = gameStateMachine;
            _tweenersLocator = tweenersLocator;
            _showLevelAnimation = showLevelAnimation;
            _ballsService = ballsService;
            _generalRestartables = generalRestartables;
            _popupService = popupService;
            _birdsService = birdsService;
            _levelDataChooser = levelDataChooser;
        }
        
        public async UniTask Enter()
        {
            await _loadingScreen.Show(false);
            await _popupService.CloseAll();
            
            _tweenersLocator.RemoveAll();

            foreach (IGeneralRestartable generalRestartable in _generalRestartables)
            {
                generalRestartable.Restart();
            }

            foreach (ICurrentLevelRestartable restartable in _currentLevelRestartables)
            {
                restartable.Restart();
            }

            _ballsService.DespawnAll();

            LevelData levelData = _levelDataChooser.GetLevelData();
            if (!levelData.NeedBird)
            {
                _birdsService.StopAll();
            }

            await _loadingScreen.Hide();
            await _showLevelAnimation.Show();
            
            _gameStateMachine.Enter<GameLoopState>().Forget();
        }

        public async UniTask Exit()
        {
            await UniTask.CompletedTask;
        }
    }
}