using System.Collections.Generic;
using App.Scripts.External.GameStateMachine;
using App.Scripts.General.Infrastructure;
using App.Scripts.General.LevelPackInfoService;
using App.Scripts.General.LoadingScreen;
using App.Scripts.General.Popup;
using App.Scripts.General.Providers;
using App.Scripts.Scenes.GameScene.Features.Dotween;
using App.Scripts.Scenes.GameScene.Features.Entities.Ball;
using App.Scripts.Scenes.GameScene.Features.Levels.General;
using App.Scripts.Scenes.GameScene.Features.Levels.General.Animations;
using App.Scripts.Scenes.GameScene.Features.Levels.General.View;
using App.Scripts.Scenes.GameScene.Features.Levels.LevelProgress;
using App.Scripts.Scenes.GameScene.Features.Levels.LevelView;
using App.Scripts.Scenes.GameScene.Features.Levels.Loading;
using App.Scripts.Scenes.GameScene.Features.Popups;
using App.Scripts.Scenes.GameScene.Features.Time;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Scenes.GameScene.States
{
    public class LoadNextLevelState : IState
    {
        private readonly ILoadingScreen _loadingScreen;
        private readonly IStateMachine _gameStateMachine;
        private readonly IPopupService _popupService;
        private readonly ILevelPackInfoView _levelPackInfoView;
        private readonly ITimeProvider _timeProvider;
        private readonly ILevelProgressService _levelProgressService;
        private readonly ITweenersLocator _tweenersLocator;
        private readonly ILevelPackInfoService _levelPackInfoService;
        private readonly IShowLevelAnimation _showLevelAnimation;
        private readonly IBallsService _ballsService;
        private readonly ILevelViewUpdater _levelViewUpdater;
        private readonly ILevelLoadService _levelLoadService;
        private readonly List<IGeneralRestartable> _generalRestartables;
        private readonly SpriteProvider _spriteProvider;

        public LoadNextLevelState(
            ILoadingScreen loadingScreen,
            IStateMachine gameStateMachine,
            IPopupService popupService,
            ILevelPackInfoView levelPackInfoView,
            ITimeProvider timeProvider,
            ILevelProgressService levelProgressService,
            ITweenersLocator tweenersLocator,
            ILevelPackInfoService levelPackInfoService,
            IShowLevelAnimation showLevelAnimation,
            IBallsService ballsService,
            ILevelLoadService levelLoadService,
            List<IGeneralRestartable> generalRestartables,
            SpriteProvider spriteProvider)
        {
            _loadingScreen = loadingScreen;
            _gameStateMachine = gameStateMachine;
            _popupService = popupService;
            _levelPackInfoView = levelPackInfoView;
            _timeProvider = timeProvider;
            _levelProgressService = levelProgressService;
            _tweenersLocator = tweenersLocator;
            _levelPackInfoService = levelPackInfoService;
            _showLevelAnimation = showLevelAnimation;
            _ballsService = ballsService;
            _levelLoadService = levelLoadService;
            _generalRestartables = generalRestartables;
            _spriteProvider = spriteProvider;
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
                Sprite = _spriteProvider.Sprites[data.LevelPack.GalacticIconKey],
                TargetScore = 0
            });

            _ballsService.DespawnAll();

            await _loadingScreen.Hide();
            await _showLevelAnimation.Show();
            
            _gameStateMachine.Enter<GameLoopState>().Forget();
        }

        private async UniTask ClosePopups()
        {
            await _popupService.Close<WinPopupView>();
            _timeProvider.TimeScale = 1f;
        }

        private void RestartAll()
        {
            foreach (IGeneralRestartable restartable in _generalRestartables)
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