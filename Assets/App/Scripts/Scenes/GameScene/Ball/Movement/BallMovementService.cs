using App.Scripts.Scenes.GameScene.Ball.Movement.MoveVariants;
using App.Scripts.Scenes.GameScene.Components;
using App.Scripts.Scenes.GameScene.Input;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Ball.Movement
{
    public sealed class BallMovementService : IBallMovementService
    {
        private readonly IBallFreeFlightMover _ballFreeFlightMover;
        private readonly ITransformable _ballTransformable;
        private readonly IClickDetector _clickDetector;

        private IBallMover _ballMover;
        private Vector2 _previousBallPosition;
        
        public BallMovementService(
            IClickDetector clickDetector,
            IBallFollowMover ballFollowMover,
            IBallFreeFlightMover ballFreeFlightMover,
            ITransformable ballTransformable
        )
        {
            _clickDetector = clickDetector;
            _ballMover = ballFollowMover;
            _ballFreeFlightMover = ballFreeFlightMover;
            _ballTransformable = ballTransformable;

            _clickDetector.MouseUp += OnMouseUp;
        }

        public void Tick()
        {
            if (_ballMover is null)
                return;

            _previousBallPosition = _ballTransformable.Position;
            _ballMover.Tick();
        }

        private void OnMouseUp()
        {
            _clickDetector.MouseUp -= OnMouseUp;

            Vector2 direction = GetDirection();

            _ballFreeFlightMover.AsyncInitialize(direction);
            _ballMover = _ballFreeFlightMover;
        }

        private Vector2 GetDirection()
        {
            return _ballTransformable.Position.x > _previousBallPosition.x
                ? new Vector2(0.5f, 0.5f)
                : new Vector2(-0.5f, 0.5f);
        }
    }
}