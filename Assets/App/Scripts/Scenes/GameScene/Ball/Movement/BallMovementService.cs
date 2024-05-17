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

        private IBallFollowMover _ballFollowMover;
        private Vector2 _previousBallPosition;
        private IBallFollowMover _ballFollowFollowMover;

        public BallMovementService(
            IClickDetector clickDetector,
            IBallFollowMover ballFollowFollowMover,
            IBallFreeFlightMover ballFreeFlightMover,
            IPositionable ballPositionable
        )
        {
            _clickDetector = clickDetector;
            _ballFollowFollowMover = ballFollowFollowMover;
            _ballFollowMover = _ballFollowFollowMover;
            _ballFreeFlightMover = ballFreeFlightMover;
            _ballPositionable = ballPositionable;

            _clickDetector.MouseUp += OnMouseUp;
        }

        public void Tick()
        {
            if (_ballFollowMover is null)
                return;

            _previousBallPosition = _ballPositionable.Position;
            _ballFollowMover.Tick();
        }

        private void OnMouseUp()
        {
            _clickDetector.MouseUp -= OnMouseUp;

            Vector2 direction = GetDirection();

            _ballFreeFlightMover.AsyncInitialize(direction);
            _ballFollowMover = null;
        }

        private Vector2 GetDirection()
        {
            return _ballPositionable.Position.x > _previousBallPosition.x
                ? new Vector2(0.5f, 0.5f)
                : new Vector2(-0.5f, 0.5f);
        }

        public void Restart()
        {
            _ballFreeFlightMover.Restart();

            _ballFollowMover = _ballFollowFollowMover;
            _ballFollowMover.AsyncInitialize();
            _clickDetector.MouseUp += OnMouseUp;
        }

        public void UpdateSpeed(float addValue)
        {
            _ballFollowMover.UpdateSpeed(addValue);
        }
    }
}