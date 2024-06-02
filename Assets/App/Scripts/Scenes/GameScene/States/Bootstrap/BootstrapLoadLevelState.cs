using System.Collections.Generic;
using App.Scripts.External.GameStateMachine;
using App.Scripts.General.Infrastructure;
using App.Scripts.Scenes.GameScene.Features.Entities.Ball;
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
        private readonly IBallsService _ballsService;
        private readonly BallView.Factory _ballViewFactory;

        public BootstrapLoadLevelState(
            IStateMachine stateMachine,
            IViewHealthPointService viewHealthPointService,
            IHealthContainer healthContainer,
            IPlayerShapeMover playerShapeMover,
            ILevelLoadService levelLoadService,
            ILevelProgressService levelProgressService,
            IBallsService ballsService,
            BallView.Factory ballViewFactory
            )
        {
            _stateMachine = stateMachine;
            _viewHealthPointService = viewHealthPointService;
            _healthContainer = healthContainer;
            _playerShapeMover = playerShapeMover;
            _levelLoadService = levelLoadService;
            _levelProgressService = levelProgressService;
            _ballsService = ballsService;
            _ballViewFactory = ballViewFactory;
        }
        
        public async UniTask Enter()
        {
            LevelData levelData = await _levelLoadService.LoadLevel();
            
            await _healthContainer.AsyncInitialize(levelData.HealthCount);
            await _viewHealthPointService.AsyncInitialize(levelData);
            _levelProgressService.CalculateStepByLevelData(levelData);

            _ballsService.AddBall(_ballViewFactory.Create());
            
            _stateMachine.Enter<BootstrapInitializeOtherServicesState>().Forget();
        }

        public async UniTask Exit()
        {
            await UniTask.CompletedTask;
        }
    }
}