using System.Collections.Generic;
using App.Scripts.External.GameStateMachine;
using App.Scripts.General.Infrastructure;
using App.Scripts.General.LevelPackInfoService;
using App.Scripts.Scenes.GameScene.Features.Entities.Ball.Movement.MoveVariants;
using App.Scripts.Scenes.GameScene.Features.Entities.Bird;
using App.Scripts.Scenes.GameScene.Features.Entities.Bird.Interfaces;
using App.Scripts.Scenes.GameScene.Features.Entities.PlayerShape.Move;
using App.Scripts.Scenes.GameScene.Features.Grid;
using App.Scripts.Scenes.GameScene.Features.Healthes;
using App.Scripts.Scenes.GameScene.Features.Levels.General;
using App.Scripts.Scenes.GameScene.Features.Levels.General.Load;
using App.Scripts.Scenes.GameScene.Features.Levels.LevelProgress;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.States.Bootstrap
{
    public class BootstrapLoadLevelState : IState
    {
        private readonly IStateMachine _stateMachine;
        private readonly ILevelPackInfoService _levelPackInfoService;
        private readonly TextAsset _sceneLevelData;
        private readonly IGridPositionResolver _gridPositionResolver;
        private readonly ILevelLoader _levelLoader;
        private readonly ILevelProgressService _levelProgressService;
        private readonly IViewHealthPointService _viewHealthPointService;
        private readonly IHealthContainer _healthContainer;
        private readonly IPlayerShapeMover _playerShapeMover;
        private readonly IBallMover _ballMover;
        private readonly IBirdsService _birdsService;
        private readonly BirdView.Factory _birdViewFactory;

        public BootstrapLoadLevelState(
            IStateMachine stateMachine,
            ILevelPackInfoService levelPackInfoService,
            TextAsset sceneLevelData,
            IGridPositionResolver gridPositionResolver,
            ILevelLoader levelLoader,
            ILevelProgressService levelProgressService,
            IViewHealthPointService viewHealthPointService,
            IHealthContainer healthContainer,
            IPlayerShapeMover playerShapeMover,
            IBirdsService birdsService,
            BirdView.Factory birdViewFactory
            )
        {
            _stateMachine = stateMachine;
            _levelPackInfoService = levelPackInfoService;
            _sceneLevelData = sceneLevelData;
            _gridPositionResolver = gridPositionResolver;
            _levelLoader = levelLoader;
            _levelProgressService = levelProgressService;
            _viewHealthPointService = viewHealthPointService;
            _healthContainer = healthContainer;
            _playerShapeMover = playerShapeMover;
            _birdsService = birdsService;
            _birdViewFactory = birdViewFactory;
        }
        
        public async UniTask Enter()
        {
            LevelData levelData = ChooseLevelData();
            
            await _gridPositionResolver.AsyncInitialize(levelData);

            LoadLevel(levelData);

            if (levelData.NeedBird)
            {
                BirdView birdView = _birdViewFactory.Create();
                _birdsService.AddBird(birdView);
                _birdsService.GoFly(birdView);
            }
            
            _stateMachine.Enter<BootstrapInitializeOtherServicesState>();
            
            await UniTask.CompletedTask;
        }

        public async UniTask Exit()
        {
            await UniTask.CompletedTask;
        }
        
        private async void LoadLevel(LevelData levelData)
        {
            _levelLoader.LoadLevel(levelData);
            
            _levelProgressService.CalculateStepByLevelData(levelData);
            await _viewHealthPointService.AsyncInitialize(levelData);
            await _healthContainer.AsyncInitialize(levelData, new List<IRestartable> {_playerShapeMover});
        }
        
        private LevelData ChooseLevelData()
        {
            var data = _levelPackInfoService.GetData();
            if (data is not null && data.NeedLoadLevel)
            {
                return JsonConvert.DeserializeObject<LevelData>(data.LevelPack.Levels[data.LevelIndex].text);
            }

            return JsonConvert.DeserializeObject<LevelData>(_sceneLevelData.text);
        }
    }
}