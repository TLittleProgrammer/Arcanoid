using App.Scripts.Scenes.GameScene.Features.Ball;
using App.Scripts.Scenes.GameScene.Features.Ball.Movement;
using App.Scripts.Scenes.GameScene.Features.Boosts.Interfaces;
using App.Scripts.Scenes.GameScene.Features.Entities;
using App.Scripts.Scenes.GameScene.Features.PlayerShape;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.Activators
{
    public class StickyBoostActivator : IConcreteBoostActivator, ITickable
    {
        private readonly PlayerView _playerView;
        private bool _isActive = false;
        
        public StickyBoostActivator(
            PlayerView playerView,
            IBoostContainer boostContainer)
        {
            _playerView = playerView;

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

            /*
            Bounds playerBounds = _playerView.BoxCollider2D.bounds;
            Bounds ballBounds   = _ballView.Collider2D.bounds;

            if (playerBounds.Intersects(ballBounds))
            {
                //_ballMovementService.Sticky();
            }*/
        }
    }
}