using System.ComponentModel;
using App.Scripts.Scenes.GameScene.Features.Ball;
using App.Scripts.Scenes.GameScene.Features.Ball.Collision;
using App.Scripts.Scenes.GameScene.Features.Ball.Movement;
using App.Scripts.Scenes.GameScene.Features.Ball.Movement.MoveVariants;
using App.Scripts.Scenes.GameScene.Features.PlayerShape;
using Cysharp.Threading.Tasks;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Factories
{
    public class BallViewFactory : IFactory<BallView>
    {
        private readonly DiContainer _sceneContext;
        private readonly BallView.Pool _ballViewPool;
        private readonly PlayerView _playerView;
        private readonly IBallsService _ballsService;

        public BallViewFactory(
            DiContainer sceneContext,
            BallView.Pool ballViewPool,
            PlayerView playerView,
            IBallsService ballsService)
        {
            _sceneContext = sceneContext;
            _ballViewPool = ballViewPool;
            _playerView = playerView;
            _ballsService = ballsService;
        }
        
        public BallView Create()
        {
            BallView ballView = _ballViewPool.Spawn();

            if(!_ballsService.Balls.ContainsKey(ballView))
            {
                IBallFollowMover ballFollowMover = _sceneContext.Instantiate<BallFollowMover>(new object[] {ballView, _playerView});
                IBallFreeFlightMover ballFreeFlightMover = _sceneContext.Instantiate<BallFreeFlight>(new[] { ballView });
                IBallCollisionService ballCollisionService = _sceneContext.Instantiate<BallCollisionService>(new[] { ballView });
                
                ballFollowMover.AsyncInitialize().Forget();
                
                IBallMovementService ballMovementService = _sceneContext.Instantiate<BallMovementService>(new object[] {ballView, ballFollowMover, ballFreeFlightMover});

                _ballsService.Balls.Add(ballView, ballMovementService);
            }
            
            return ballView;
        }
    }
}