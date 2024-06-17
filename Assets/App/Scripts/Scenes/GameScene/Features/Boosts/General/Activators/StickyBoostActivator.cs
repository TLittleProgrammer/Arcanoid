using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.Features.Entities.Ball;
using App.Scripts.Scenes.GameScene.Features.Entities.EntityDestroyer;
using App.Scripts.Scenes.GameScene.Features.Entities.PlayerShape;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.General.Activators
{
    public class StickyBoostActivator : IConcreteBoostActivator
    {
        private readonly IBallsService _ballsService;
        
        private bool _isActive;
        
        public StickyBoostActivator(IBallsService ballsService)
        {
            _ballsService = ballsService;
        }
        
        public bool IsTimeableBoost => true;

        public void Activate()
        {
            _ballsService.BallAdded += OnBallAdded;
            _isActive = true;
            
            foreach (BallView view in _ballsService.Balls)
            {
                view.Collidered += OnCollidered;
            }
        }
        
        public void Deactivate()
        {
            _ballsService.BallAdded -= OnBallAdded;
            _isActive = false;

            foreach (BallView view in _ballsService.Balls)
            {
                _ballsService.Fly(view);
                view.Collidered -= OnCollidered;
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
    }
}