using System.Collections.Generic;
using App.Scripts.External.GameStateMachine;
using App.Scripts.General.Infrastructure;
using App.Scripts.Scenes.GameScene.Features.Entities.Ball.Movement.MoveVariants;
using App.Scripts.Scenes.GameScene.Features.Entities.PlayerShape.Move;
using App.Scripts.Scenes.GameScene.Features.Healthes;
using App.Scripts.Scenes.GameScene.Features.Levels.General;
using App.Scripts.Scenes.GameScene.Features.Levels.LevelProgress;
using App.Scripts.Scenes.GameScene.Features.Levels.Loading;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Scenes.GameScene.States.Bootstrap
{
    public class BootstrapLoadLevelState : IState
    {
        private readonly IStateMachine _stateMachine;
        private readonly IViewHealthPointService _viewHealthPointService;
        private readonly IHealthContainer _healthContainer;
        private readonly IPlayerShapeMover _playerShapeMover;
        private readonly IBallMover _ballMover;
        private readonly ILevelLoadService _levelLoadService;
        private readonly ILevelProgressService _levelProgressService;

        public BootstrapLoadLevelState(
            IStateMachine stateMachine,
            IViewHealthPointService viewHealthPointService,
            IHealthContainer healthContainer,
            IPlayerShapeMover playerShapeMover,
            ILevelLoadService levelLoadService,
            ILevelProgressService levelProgressService
            )
        {
            _stateMachine = stateMachine;
            _viewHealthPointService = viewHealthPointService;
            _healthContainer = healthContainer;
            _playerShapeMover = playerShapeMover;
            _levelLoadService = levelLoadService;
            _levelProgressService = levelProgressService;
        }
        
        public async UniTask Enter()
        {
            LevelData levelData = await _levelLoadService.LoadLevel();
            
            await _healthContainer.AsyncInitialize(levelData, new List<IRestartable> {_playerShapeMover});
            await _viewHealthPointService.AsyncInitialize(levelData);
            _levelProgressService.CalculateStepByLevelData(levelData);
            
            _stateMachine.Enter<BootstrapInitializeOtherServicesState>();
            
            await UniTask.CompletedTask;
        }

        public async UniTask Exit()
        {
            await UniTask.CompletedTask;
        }
    }
}