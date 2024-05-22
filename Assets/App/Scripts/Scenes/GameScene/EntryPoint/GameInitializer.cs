﻿using App.Scripts.External.GameStateMachine;
using App.Scripts.General.Constants;
using App.Scripts.General.Infrastructure;
using App.Scripts.General.LevelPackInfoService;
using App.Scripts.General.Levels;
using App.Scripts.General.Popup.AssetManagment;
using App.Scripts.Scenes.GameScene.Ball;
using App.Scripts.Scenes.GameScene.Ball.Movement;
using App.Scripts.Scenes.GameScene.Grid;
using App.Scripts.Scenes.GameScene.Healthes;
using App.Scripts.Scenes.GameScene.LevelProgress;
using App.Scripts.Scenes.GameScene.Levels;
using App.Scripts.Scenes.GameScene.Levels.Load;
using App.Scripts.Scenes.GameScene.PlayerShape.Move;
using App.Scripts.Scenes.GameScene.States;
using App.Scripts.Scenes.GameScene.Walls;
using Newtonsoft.Json;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Installers
{
    public class GameInitializer : IInitializable
    {
        private readonly IGridPositionResolver _gridPositionResolver;
        private readonly IPopupProvider _popupProvider;
        private readonly IBallSpeedUpdater _ballSpeedUpdater;
        private readonly IBallMovementService _ballMovementService;
        private readonly ILevelLoader _levelLoader;
        private readonly ILevelProgressService _levelProgressService;
        private readonly IHealthContainer _healthContainer;
        private readonly IHealthPointService _healthPointService;
        private readonly IPlayerShapeMover _playerShapeMover;
        private readonly IWallLoader _wallLoader;
        private readonly TextAsset _levelData;
        private readonly ILevelPackInfoService _levelPackInfoService;
        private readonly GameLoopState _gameLoopState;
        private readonly WinState _winState;
        private readonly LoadSceneFromMainMenuState _loadSceneFromMainMenuState;
        private readonly RestartState _restartState;
        private readonly LoadNextLevelState _loadNextLevelState;
        private readonly PopupState _popupState;
        private readonly LooseState _looseState;
        private readonly IStateMachine _stateMachine;

        public GameInitializer(
            IGridPositionResolver gridPositionResolver,
            IPopupProvider popupProvider,
            IBallSpeedUpdater ballSpeedUpdater,
            IBallMovementService ballMovementService,
            ILevelLoader levelLoader,
            ILevelProgressService levelProgressService,
            IHealthContainer healthContainer,
            IHealthPointService healthPointService,
            IPlayerShapeMover playerShapeMover,
            IWallLoader wallLoader,
            TextAsset levelData,
            ILevelPackInfoService levelPackInfoService,
            GameLoopState gameLoopState,
            WinState winState,
            LoadSceneFromMainMenuState loadSceneFromMainMenuState,
            RestartState restartState,
            LoadNextLevelState loadNextLevelState,
            PopupState popupState,
            LooseState looseState,
            IStateMachine stateMachine)
        {
            _gridPositionResolver = gridPositionResolver;
            _popupProvider = popupProvider;
            _ballSpeedUpdater = ballSpeedUpdater;
            _ballMovementService = ballMovementService;
            _levelLoader = levelLoader;
            _levelProgressService = levelProgressService;
            _healthContainer = healthContainer;
            _healthPointService = healthPointService;
            _playerShapeMover = playerShapeMover;
            _wallLoader = wallLoader;
            _levelData = levelData;
            _levelPackInfoService = levelPackInfoService;
            _gameLoopState = gameLoopState;
            _winState = winState;
            _loadSceneFromMainMenuState = loadSceneFromMainMenuState;
            _restartState = restartState;
            _loadNextLevelState = loadNextLevelState;
            _popupState = popupState;
            _looseState = looseState;
            _stateMachine = stateMachine;
        }
        
        public async void Initialize()
        {
            LevelData levelData = ChooseLevelData();
            
            await _gridPositionResolver.AsyncInitialize(levelData);

            LoadLevel(levelData);
            await _wallLoader.AsyncInitialize();
            await _popupProvider.AsyncInitialize(Pathes.PathToPopups);
            await _ballSpeedUpdater.AsyncInitialize(_ballMovementService);

            InitializeStateMachine();
        }

        private void InitializeStateMachine()
        {
            IExitableState[] states =
            {
                _gameLoopState,
                _winState,
                _loadSceneFromMainMenuState,
                _restartState,
                _loadNextLevelState,
                _popupState,
                _looseState,
            };

            _stateMachine.AsyncInitialize(states);
            
            _stateMachine.Enter<GameLoopState>();
        }

        private LevelData ChooseLevelData()
        {
            var data = _levelPackInfoService.GetData();
            if (data.NeedLoadLevel)
            {
                return JsonConvert.DeserializeObject<LevelData>(data.LevelPack.Levels[data.LevelIndex].text);
            }

            return JsonConvert.DeserializeObject<LevelData>(_levelData.text);
        }
        
        private async void LoadLevel(LevelData levelData)
        {
            _levelLoader.LoadLevel(levelData);
            
            _levelProgressService.CalculateStepByLevelData(levelData);
            await _healthPointService.AsyncInitialize(levelData);
            await _healthContainer.AsyncInitialize(levelData, new IRestartable[] {_ballMovementService, _playerShapeMover});
        }
    }
}