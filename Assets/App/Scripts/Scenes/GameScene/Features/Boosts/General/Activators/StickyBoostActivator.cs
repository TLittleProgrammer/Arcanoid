using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.Features.Boosts.General.Interfaces;
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
        private List<BallView> _balls;
        
        public StickyBoostActivator(IBallsService ballsService)
        {
            _ballsService = ballsService;
            _ballsService.BallAdded += OnBallAdded;
            _balls = new();
        }
        
        public bool IsTimeableBoost => true;

        public void Activate(IBoostDataProvider boostDataProvider)
        {
            _isActive = true;
            
            foreach (BallView view in _balls)
            {
                view.Collidered += OnCollidered;
            }
        }
        
        public void Deactivate()
        {
            _isActive = false;

            foreach (BallView view in _balls)
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
            if (_balls is not null && !_balls.Contains(view))
            {
                _balls.Add(view);

                if (_isActive)
                {
                    view.Collidered += OnCollidered;
                }
            }
        }
    }
}