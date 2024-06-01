using App.Scripts.Scenes.GameScene.Features.Entities.Ball;
using App.Scripts.Scenes.GameScene.Features.Entities.Ball.Collision;
using App.Scripts.Scenes.GameScene.Features.Entities.Ball.Movement;
using App.Scripts.Scenes.GameScene.Features.Entities.Ball.Movement.MoveVariants;
using App.Scripts.Scenes.GameScene.Features.Entities.PlayerShape;
using Cysharp.Threading.Tasks;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Factories
{
    public sealed class BallMovementServiceFactory : IFactory<BallView, IBallMovementService>
    {
        private readonly DiContainer _sceneContext;
        private readonly PlayerView _playerView;

        public BallMovementServiceFactory(DiContainer sceneContext, PlayerView playerView)
        {
            _sceneContext = sceneContext;
            _playerView = playerView;
        }
        
        public IBallMovementService Create(BallView ballView)
        {
            IBallFollowMover ballFollowMover = _sceneContext.Instantiate<BallFollowMover>(new object[] {ballView, _playerView});
            IBallFreeFlightMover ballFreeFlightMover = _sceneContext.Instantiate<BallFreeFlight>(new[] { ballView });
            _sceneContext.Instantiate<BallCollisionService>(new[] { ballView });
                
            ballFollowMover.AsyncInitialize().Forget();
                
            IBallMovementService ballMovementService = _sceneContext.Instantiate<BallMovementService>(new object[] {ballFollowMover, ballFreeFlightMover});

            return ballMovementService;
        }
    }
}