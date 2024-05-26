using App.Scripts.External.GameStateMachine;
using App.Scripts.General.Constants;
using App.Scripts.General.Infrastructure;
using App.Scripts.General.LevelPackInfoService;
using App.Scripts.General.Popup.AssetManagment;
using App.Scripts.Scenes.GameScene.Features.Ball;
using App.Scripts.Scenes.GameScene.Features.Ball.Movement;
using App.Scripts.Scenes.GameScene.Features.Grid;
using App.Scripts.Scenes.GameScene.Features.Healthes;
using App.Scripts.Scenes.GameScene.Features.LevelProgress;
using App.Scripts.Scenes.GameScene.Features.Levels;
using App.Scripts.Scenes.GameScene.Features.Levels.Animations;
using App.Scripts.Scenes.GameScene.Features.Levels.Load;
using App.Scripts.Scenes.GameScene.Features.PlayerShape.Move;
using App.Scripts.Scenes.GameScene.Features.States;
using App.Scripts.Scenes.GameScene.Features.Walls;
using Newtonsoft.Json;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene.EntryPoint
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
        private readonly IShowLevelAnimation _showLevelAnimation;

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
            IStateMachine stateMachine,
            IShowLevelAnimation showLevelAnimation)
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
            _showLevelAnimation = showLevelAnimation;
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

        private async void InitializeStateMachine()
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

            await _stateMachine.AsyncInitialize(states);
            await _showLevelAnimation.Show();
            
            _stateMachine.Enter<GameLoopState>();
        }

        private LevelData ChooseLevelData()
        {
            var data = _levelPackInfoService.GetData();
            if (data is not null && data.NeedLoadLevel)
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
            await _healthContainer.AsyncInitialize(levelData, new IRestartable[] {_playerShapeMover, _ballMovementService});
        }
    }
}