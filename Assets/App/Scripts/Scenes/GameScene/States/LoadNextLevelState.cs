using System.Collections.Generic;
using System.Threading.Tasks;
using App.Scripts.External.GameStateMachine;
using App.Scripts.General.Infrastructure;
using App.Scripts.General.LevelPackInfoService;
using App.Scripts.General.LoadingScreen;
using App.Scripts.General.Popup;
using App.Scripts.Scenes.GameScene.Features.Dotween;
using App.Scripts.Scenes.GameScene.Features.Entities.Ball;
using App.Scripts.Scenes.GameScene.Features.Entities.Bird;
using App.Scripts.Scenes.GameScene.Features.Entities.Bird.Interfaces;
using App.Scripts.Scenes.GameScene.Features.Grid;
using App.Scripts.Scenes.GameScene.Features.Levels.General;
using App.Scripts.Scenes.GameScene.Features.Levels.General.Animations;
using App.Scripts.Scenes.GameScene.Features.Levels.General.Load;
using App.Scripts.Scenes.GameScene.Features.Levels.General.View;
using App.Scripts.Scenes.GameScene.Features.Levels.LevelProgress;
using App.Scripts.Scenes.GameScene.Features.Levels.LevelView;
using App.Scripts.Scenes.GameScene.Features.Levels.Loading;
using App.Scripts.Scenes.GameScene.Features.Popups;
using App.Scripts.Scenes.GameScene.Features.Time;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;

namespace App.Scripts.Scenes.GameScene.States
{
    public class LoadNextLevelState : IState
    {
        private readonly ILoadingScreen _loadingScreen;
        private readonly IEnumerable<IRestartable> _restartables;
        private readonly IStateMachine _gameStateMachine;
        private readonly ILevelLoader _levelLoader;
        private readonly IPopupService _popupService;
        private readonly ILevelPackInfoView _levelPackInfoView;
        private readonly ITimeProvider _timeProvider;
        private readonly IGridPositionResolver _gridPositionResolver;
        private readonly ILevelProgressService _levelProgressService;
        private readonly ITweenersLocator _tweenersLocator;
        private readonly ILevelPackInfoService _levelPackInfoService;
        private readonly IShowLevelAnimation _showLevelAnimation;
        private readonly IBallsService _ballsService;
        private readonly BallView.Pool _ballViewPool;
        private readonly ILevelViewUpdater _levelViewUpdater;
        private readonly IBirdsService _birdsService;
        private readonly BirdView.Factory _birdViewFactory;
        private readonly ILevelLoadService _levelLoadService;

        public LoadNextLevelState(
            ILoadingScreen loadingScreen,
            IEnumerable<IRestartable> restartables,
            IStateMachine gameStateMachine,
            ILevelLoader levelLoader,
            IPopupService popupService,
            ILevelPackInfoView levelPackInfoView,
            ITimeProvider timeProvider,
            IGridPositionResolver gridPositionResolver,
            ILevelProgressService levelProgressService,
            ITweenersLocator tweenersLocator,
            ILevelPackInfoService levelPackInfoService,
            IShowLevelAnimation showLevelAnimation,
            IBallsService ballsService,
            BallView.Pool ballViewPool,
            IBirdsService birdsService,
            BirdView.Factory birdViewFactory,
            ILevelLoadService levelLoadService)
        {
            _loadingScreen = loadingScreen;
            _restartables = restartables;
            _gameStateMachine = gameStateMachine;
            _levelLoader = levelLoader;
            _popupService = popupService;
            _levelPackInfoView = levelPackInfoView;
            _timeProvider = timeProvider;
            _gridPositionResolver = gridPositionResolver;
            _levelProgressService = levelProgressService;
            _tweenersLocator = tweenersLocator;
            _levelPackInfoService = levelPackInfoService;
            _showLevelAnimation = showLevelAnimation;
            _ballsService = ballsService;
            _ballViewPool = ballViewPool;
            _birdsService = birdsService;
            _birdViewFactory = birdViewFactory;
            _levelLoadService = levelLoadService;
        }

        public async UniTask Enter()
        {
            await _loadingScreen.Show(false);

            _tweenersLocator.RemoveAll();
            RestartAll();
            
            await ClosePopups();
            LevelData level = await _levelLoadService.LoadLevelNextLevel();

            var data = _levelPackInfoService.GetData();
            
            _levelProgressService.CalculateStepByLevelData(level);
            
            _levelPackInfoView.Initialize(new LevelPackInfoRecord
            {
                AllLevelsCountFromPack = data.LevelPack.Levels.Count,
                CurrentLevelIndex = data.LevelIndex,
                Sprite = data.LevelPack.GalacticIcon,
                TargetScore = 0
            });

            _ballsService.DespawnAll();

            await _loadingScreen.Hide();
            await _showLevelAnimation.Show();
            
            _gameStateMachine.Enter<GameLoopState>();
        }

        private async UniTask ClosePopups()
        {
            await _popupService.Close<WinPopupView>();
            _timeProvider.TimeScale = 1f;
        }

        private void RestartAll()
        {
            foreach (IRestartable restartable in _restartables)
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