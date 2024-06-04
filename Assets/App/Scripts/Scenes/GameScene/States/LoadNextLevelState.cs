﻿using System.Collections.Generic;
using App.Scripts.External.GameStateMachine;
using App.Scripts.External.UserData;
using App.Scripts.General.Infrastructure;
using App.Scripts.General.Levels.LevelPackInfoService;
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
using App.Scripts.Scenes.GameScene.Features.Levels.SavedLevelProgress;
using App.Scripts.Scenes.GameScene.Features.Popups;
using App.Scripts.Scenes.GameScene.Features.Time;
using App.Scripts.Scenes.GameScene.MVVM.Header;
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
        private readonly ILevelProgressService _levelProgressService;
        private readonly ITweenersLocator _tweenersLocator;
        private readonly ILevelPackInfoService _levelPackInfoService;
        private readonly IShowLevelAnimation _showLevelAnimation;
        private readonly IBallsService _ballsService;
        private readonly ILevelViewUpdater _levelViewUpdater;
        private readonly ILevelLoadService _levelLoadService;
        private readonly List<IGeneralRestartable> _generalRestartables;
        private readonly List<ICurrentLevelRestartable> _currentLevelRestartables;
        private readonly SpriteProvider _spriteProvider;
        private readonly IDataProvider<LevelDataProgress> _levelDataProgress;
        private readonly LevelPackInfoViewModel _levelPackInfoViewModel;

        public LoadNextLevelState(
            ILoadingScreen loadingScreen,
            IStateMachine gameStateMachine,
            IPopupService popupService,
            ITimeProvider timeProvider,
            ILevelProgressService levelProgressService,
            ITweenersLocator tweenersLocator,
            ILevelPackInfoService levelPackInfoService,
            IShowLevelAnimation showLevelAnimation,
            IBallsService ballsService,
            ILevelLoadService levelLoadService,
            List<IGeneralRestartable> generalRestartables,
            List<ICurrentLevelRestartable> currentLevelRestartables,
            SpriteProvider spriteProvider,
            IDataProvider<LevelDataProgress> levelDataProgress,
            LevelPackInfoViewModel levelPackInfoViewModel)
        {
            _loadingScreen = loadingScreen;
            _gameStateMachine = gameStateMachine;
            _popupService = popupService;
            _timeProvider = timeProvider;
            _levelProgressService = levelProgressService;
            _tweenersLocator = tweenersLocator;
            _levelPackInfoService = levelPackInfoService;
            _showLevelAnimation = showLevelAnimation;
            _ballsService = ballsService;
            _levelLoadService = levelLoadService;
            _generalRestartables = generalRestartables;
            _currentLevelRestartables = currentLevelRestartables;
            _spriteProvider = spriteProvider;
            _levelDataProgress = levelDataProgress;
            _levelPackInfoViewModel = levelPackInfoViewModel;
        }

        public async UniTask Enter()
        {
            await _loadingScreen.Show(false);
            _levelDataProgress.GetData();
            
            _tweenersLocator.RemoveAll();
            RestartAll();
            
            await ClosePopups();

            LevelData level = await _levelLoadService.LoadLevelNextLevel();

            var data = _levelPackInfoService.LevelPackTransferData;
            
            _levelProgressService.CalculateStepByLevelData(level);
            
            _levelPackInfoViewModel.UpdateView(new LevelPackInfoRecord
            {
                AllLevelsCountFromPack = data.LevelPack.Levels.Count,
                CurrentLevelIndex = data.LevelIndex,
                GalacticIconSprite = _spriteProvider.Sprites[data.LevelPack.GalacticIconKey],
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