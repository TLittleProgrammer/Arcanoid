using App.Scripts.Scenes.GameScene.Ball.Movement.MoveVariants;
using App.Scripts.Scenes.GameScene.Components;
using App.Scripts.Scenes.GameScene.Input;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Ball.Movement
{
    public sealed class BallMovementService : IBallMovementService
    {
        private readonly IBallFreeFlightMover _ballFreeFlightMover;
        private readonly IPositionable _ballPositionable;
        private readonly IClickDetector _clickDetector;

        private IBallMover _ballMover;
        private Vector2 _previousBallPosition;
        private IBallFollowMover _ballFollowMover;

        public BallMovementService(
            IClickDetector clickDetector,
            IBallFollowMover ballFollowMover,
            IBallFreeFlightMover ballFreeFlightMover,
            IPositionable ballPositionable
        )
        {
            _ballFollowMover = ballFollowMover;
            _clickDetector = clickDetector;
            _ballMover = ballFollowMover;
            _ballFreeFlightMover = ballFreeFlightMover;
            _ballPositionable = ballPositionable;

            _clickDetector.MouseUp += OnMouseUp;
        }

        public void Tick()
        {
            if (_ballMover is null)
                return;

            _previousBallPosition = _ballPositionable.Position;
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
            return _ballPositionable.Position.x > _previousBallPosition.x
                ? new Vector2(0.5f, 0.5f)
                : new Vector2(-0.5f, 0.5f);
        }

        public void Restart()
        {
            _ballFollowMover.AsyncInitialize();
            _ballMover = _ballFollowMover;
            _clickDetector.MouseUp += OnMouseUp;
        }
    }
}