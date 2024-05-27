using App.Scripts.Scenes.GameScene.Features.Ball.Movement.MoveVariants;
using App.Scripts.Scenes.GameScene.Features.Components;
using App.Scripts.Scenes.GameScene.Features.Input;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Ball.Movement
{
    public sealed class BallMovementService : IBallMovementService
    {
        private readonly IBallFreeFlightMover _ballFreeFlightMover;
        private readonly IPositionable _ballPositionable;
        private readonly IBallsService _ballsService;
        private readonly IClickDetector _clickDetector;

        private IBallFollowMover _ballFollowMover;
        private IBallFollowMover _ballFollowFollowMover;

        public BallMovementService(
            IClickDetector clickDetector,
            IBallFollowMover ballFollowFollowMover,
            IBallFreeFlightMover ballFreeFlightMover,
            IPositionable ballPositionable,
            IBallsService ballsService
        )
        {
            _clickDetector = clickDetector;
            _ballFollowFollowMover = ballFollowFollowMover;
            _ballFollowMover = _ballFollowFollowMover;
            _ballFreeFlightMover = ballFreeFlightMover;
            _ballPositionable = ballPositionable;
            _ballsService = ballsService;

            _clickDetector.MouseUp += OnMouseUp;
        }

        public void Tick()
        {
            if (_ballFollowMover is null)
                return;

            _ballFollowMover.Tick();
        }

        private void OnMouseUp()
        {
            _clickDetector.MouseUp -= OnMouseUp;

            _ballFreeFlightMover.AsyncInitialize(Vector2.up);
            _ballFollowMover = null;
        }

        public void Restart()
        {
            _ballFreeFlightMover.Reset();

            _ballFollowMover = _ballFollowFollowMover;
            _ballFollowMover.AsyncInitialize();
            _clickDetector.MouseUp += OnMouseUp;

            _ballsService.AddBall(_ballPositionable);
        }

        public void UpdateSpeed(float addValue)
        {
            _ballFreeFlightMover.UpdateSpeed(addValue);
        }

        public void Sticky()
        {
            _ballFreeFlightMover.Restart();

            _ballFollowMover = _ballFollowFollowMover;
            _ballFollowMover.Restart();
            _clickDetector.MouseUp += OnMouseUp;
        }
    }
}