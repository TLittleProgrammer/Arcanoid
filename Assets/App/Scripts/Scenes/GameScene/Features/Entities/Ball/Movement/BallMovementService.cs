using App.Scripts.Scenes.GameScene.Features.Constants;
using App.Scripts.Scenes.GameScene.Features.Entities.Ball.Movement.MoveVariants;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Entities.Ball.Movement
{
    public sealed class BallMovementService : IBallMovementService
    {
        private readonly IBallFreeFlightMover _ballFreeFlightMover;

        private IBallFollowMover _ballFollowMover;
        private IBallFollowMover _ballFollowFollowMover;

        public BallMovementService(IBallFollowMover ballFollowFollowMover, IBallFreeFlightMover ballFreeFlightMover)
        {
            _ballFollowFollowMover = ballFollowFollowMover;
            _ballFollowMover = _ballFollowFollowMover;
            
            _ballFreeFlightMover = ballFreeFlightMover;
            IsFreeFlight = false;
        }

        public bool IsFreeFlight { get; set; }

        public void Tick()
        {
            if (_ballFollowMover is null)
                return;

            _ballFollowMover.Tick();
        }

        public void Restart()
        {
            _ballFreeFlightMover.Reset();

            _ballFollowMover = _ballFollowFollowMover;
            _ballFollowMover.AsyncInitialize();
            IsFreeFlight = false;
        }

        public void UpdateSpeed(float progress)
        {
            _ballFreeFlightMover.SetSpeed(Mathf.Lerp(5f, GameConstants.MaxBallSpeed, progress));
        }

        public void GoFly()
        {
            IsFreeFlight = true;
            _ballFreeFlightMover.AsyncInitialize(Vector2.up);
            _ballFollowMover = null;
        }

        public void Sticky()
        {
            _ballFreeFlightMover.Restart();

            _ballFollowMover = _ballFollowFollowMover;
            _ballFollowMover.Restart();

            IsFreeFlight = false;
        }

        public void SetSpeedMultiplier(float speedMultiplier)
        {
            _ballFreeFlightMover.SetSpeedMultiplier(speedMultiplier);
        }
    }
}