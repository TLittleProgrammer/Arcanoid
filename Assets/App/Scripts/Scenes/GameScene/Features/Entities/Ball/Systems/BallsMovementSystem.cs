using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.Features.Components;
using App.Scripts.Scenes.GameScene.Features.Entities.Ball.Movement;
using App.Scripts.Scenes.GameScene.Features.Factories.Ball;

namespace App.Scripts.Scenes.GameScene.Features.Entities.Ball.Systems
{
    public sealed class BallsMovementSystem : IBallsMovementSystem, IActivable
    {
        private readonly BallMovementFactory _ballMovementFactory;
        
        private Dictionary<BallView, IBallMovementService> _balls = new();

        public BallsMovementSystem(BallMovementFactory ballMovementFactory)
        {
            _ballMovementFactory = ballMovementFactory;
        }
        
        public bool IsActive { get; set; }

        public void Tick()
        {
            if (!IsActive)
                return;

            foreach ((BallView view, IBallMovementService movementService) in _balls)
            {
                if (view.gameObject.activeSelf)
                {
                    movementService.Tick();
                }
            }
        }

        public void AddBall(BallView ballView)
        {
            if (!_balls.ContainsKey(ballView))
            {
                CreateMovementServiceForBall(ballView);
            }
        }

        public IBallMovementService GetMovementService(BallView ballView)
        {
            return _balls[ballView];
        }

        private void CreateMovementServiceForBall(BallView ballView)
        {
            IBallMovementService ballMovement = _ballMovementFactory.Create(ballView);
            _balls.Add(ballView, ballMovement);
        }
    }
}