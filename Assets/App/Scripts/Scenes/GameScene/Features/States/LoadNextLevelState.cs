﻿using System.Collections.Generic;
using App.Scripts.External.GameStateMachine;
using App.Scripts.General.Infrastructure;
using App.Scripts.General.LevelPackInfoService;
using App.Scripts.General.LoadingScreen;
using App.Scripts.General.Popup;
using App.Scripts.Scenes.GameScene.Features.Dotween;
using App.Scripts.Scenes.GameScene.Features.Grid;
using App.Scripts.Scenes.GameScene.Features.LevelProgress;
using App.Scripts.Scenes.GameScene.Features.Levels;
using App.Scripts.Scenes.GameScene.Features.Levels.Load;
using App.Scripts.Scenes.GameScene.Features.Levels.View;
using App.Scripts.Scenes.GameScene.Features.LevelView;
using App.Scripts.Scenes.GameScene.Features.Popups;
using App.Scripts.Scenes.GameScene.Features.Time;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;

namespace App.Scripts.Scenes.GameScene.Features.States
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
        private readonly ILevelViewUpdater _levelViewUpdater;
        

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
            ILevelPackInfoService levelPackInfoService)
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
        }

        public async UniTask Enter()
        {
            await _loadingScreen.Show(false);

            RemoveTweeners();
            RestartAll();
            await ClosePopups();

            var data  = _levelPackInfoService.UpdateLevelPackTransferData();
            LevelData levelData = JsonConvert.DeserializeObject<LevelData>(data.LevelPack.Levels[data.LevelIndex].text);

            await _gridPositionResolver.AsyncInitialize(levelData);
            _levelProgressService.CalculateStepByLevelData(levelData);
            
            _levelLoader.LoadLevel(levelData);
            _levelPackInfoView.Initialize(new LevelPackInfoRecord
            {
                AllLevelsCountFromPack = data.LevelPack.Levels.Count,
                CurrentLevelIndex = data.LevelIndex,
                Sprite = data.LevelPack.GalacticIcon,
                TargetScore = 0
            });
            
            _gameStateMachine.Enter<GameLoopState>();
        }

        private async UniTask ClosePopups()
        {
            await _popupService.Close<WinPopupView>();
            _timeProvider.TimeScale = 1f;
        }

        private void RemoveTweeners()
        {
            _tweenersLocator.RemoveAll();
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
            await _loadingScreen.Hide();
        }
    }
}