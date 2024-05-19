using App.Scripts.General.Constants;
using App.Scripts.General.Infrastructure;
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
        private readonly ILevelPackTransferData _levelPackTransferData;
        private readonly ILevelLoader _levelLoader;
        private readonly ILevelProgressService _levelProgressService;
        private readonly IHealthContainer _healthContainer;
        private readonly IHealthPointService _healthPointService;
        private readonly IPlayerShapeMover _playerShapeMover;
        private readonly IWallLoader _wallLoader;
        private readonly TextAsset _levelData;

        public GameInitializer(
            IGridPositionResolver gridPositionResolver,
            IPopupProvider popupProvider,
            IBallSpeedUpdater ballSpeedUpdater,
            IBallMovementService ballMovementService,
            ILevelPackTransferData levelPackTransferData,
            ILevelLoader levelLoader,
            ILevelProgressService levelProgressService,
            IHealthContainer healthContainer,
            IHealthPointService healthPointService,
            IPlayerShapeMover playerShapeMover,
            IWallLoader wallLoader,
            TextAsset levelData)
        {
            _gridPositionResolver = gridPositionResolver;
            _popupProvider = popupProvider;
            _ballSpeedUpdater = ballSpeedUpdater;
            _ballMovementService = ballMovementService;
            _levelPackTransferData = levelPackTransferData;
            _levelLoader = levelLoader;
            _levelProgressService = levelProgressService;
            _healthContainer = healthContainer;
            _healthPointService = healthPointService;
            _playerShapeMover = playerShapeMover;
            _wallLoader = wallLoader;
            _levelData = levelData;
        }
        
        public async void Initialize()
        {
            LevelData levelData = ChooseLevelData();
            
            await _gridPositionResolver.AsyncInitialize(levelData);

            LoadLevel(levelData);
            await _wallLoader.AsyncInitialize();
            await _popupProvider.AsyncInitialize(Pathes.PathToPopups);
            await _ballSpeedUpdater.AsyncInitialize(_ballMovementService);
        }
    
        private LevelData ChooseLevelData()
        {
            if (_levelPackTransferData.NeedLoadLevel)
            {
                return JsonConvert.DeserializeObject<LevelData>(_levelPackTransferData.LevelPack.Levels[_levelPackTransferData.LevelIndex].text);
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