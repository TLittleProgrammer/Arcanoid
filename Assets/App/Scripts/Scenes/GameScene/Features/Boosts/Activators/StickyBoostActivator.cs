using App.Scripts.Scenes.GameScene.Features.Boosts.Interfaces;
using App.Scripts.Scenes.GameScene.Features.Entities;
using App.Scripts.Scenes.GameScene.Features.Entities.Ball;
using App.Scripts.Scenes.GameScene.Features.Entities.PlayerShape;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.Activators
{
    public class StickyBoostActivator : IConcreteBoostActivator
    {
        private readonly IBallsService _ballsService;
        private bool _isActive;
        
        public StickyBoostActivator(IBoostContainer boostContainer, IBallsService ballsService)
        {
            _ballsService = ballsService;

            _ballsService.BallAdded += OnBallAdded;
            boostContainer.BoostEnded += OnBoostEnded;
        }

        public void Activate(BoostTypeId boostTypeId)
        {
            _isActive = true;

            foreach ((BallView view, var garbarge)in _ballsService.Balls)
            {
                view.Collidered += OnCollidered;
            }
        }

        private void OnCollidered(BallView view, Collider2D obj)
        {
            if (_isActive && obj.TryGetComponent(out PlayerView playerView))
            {
                _ballsService.SetSticky(view);
            }
        }

        private void OnBallAdded(BallView view)
        {
            if (_isActive)
            {
                view.Collidered += OnCollidered;
            }
        }

        private void OnBoostEnded(BoostTypeId boostType)
        {
            if (boostType is BoostTypeId.StickyPlatform)
            {
                _isActive = false;

                foreach ((BallView view, var garbarge) in _ballsService.Balls)
                {
                    _ballsService.Fly(view);
                    view.Collidered -= OnCollidered;
                }
            }
        }
    }
}