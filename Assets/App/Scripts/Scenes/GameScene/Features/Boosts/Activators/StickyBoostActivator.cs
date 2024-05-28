using App.Scripts.Scenes.GameScene.Features.Ball;
using App.Scripts.Scenes.GameScene.Features.Ball.Movement;
using App.Scripts.Scenes.GameScene.Features.Boosts.Interfaces;
using App.Scripts.Scenes.GameScene.Features.Entities;
using App.Scripts.Scenes.GameScene.Features.PlayerShape;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.Helpers
{
    public class StickyBoostActivator : IConcreteBoostActivator, ITickable
    {
        private readonly PlayerView _playerView;
        private readonly BallView _ballView;
        private readonly IBallMovementService _ballMovementService;
        private bool _isActive = false;
        
        public StickyBoostActivator(
            PlayerView playerView,
            BallView ballView,
            IBoostContainer boostContainer,
            IBallMovementService ballMovementService)
        {
            _playerView = playerView;
            _ballView = ballView;
            _ballMovementService = ballMovementService;

            boostContainer.BoostEnded += OnBoostEnded;
        }

        public void Activate(BoostTypeId boostTypeId)
        {
            _isActive = true;
        }

        private void OnBoostEnded(BoostTypeId boostType)
        {
            _isActive = false;
        }

        public void Tick()
        {
            if (_isActive is false)
            {
                return;
            }

            Bounds playerBounds = _playerView.BoxCollider2D.bounds;
            Bounds ballBounds   = _ballView.Collider2D.bounds;

            if (playerBounds.Intersects(ballBounds))
            {
                _ballMovementService.Sticky();
            }
        }
    }
}