using System.Runtime.Serialization;
using App.Scripts.Scenes.GameScene.Features.Boosts.General;
using App.Scripts.Scenes.GameScene.Features.Entities.Ball;
using App.Scripts.Scenes.GameScene.Features.Entities.EntityDestroyer.Helpers;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Entities.PlayerShape.Collisions
{
    public class PlayerCollisionService
    {
        private readonly IBoostsActivator _boostsActivator;
        private readonly SimpleDestroyService _simpleDestroyService;
        private readonly IBallsService _ballsService;
        private Sequence _sequence;
        private readonly TweenerCore<Vector3,Vector3,VectorOptions> _tweener;
        private PlayerView _playerView;
        private readonly float _initialY;

        public PlayerCollisionService(
            PlayerView playerView,
            IBoostsActivator boostsActivator,
            SimpleDestroyService simpleDestroyService,
            IBallsService ballsService)
        {
            _playerView = playerView;
            _boostsActivator = boostsActivator;
            _simpleDestroyService = simpleDestroyService;
            _ballsService = ballsService;
            playerView.Collided += OnCollided;
            _initialY = playerView.transform.position.y;
        }

        private void OnCollided(Collision2D obj)
        {
            if (obj.collider.TryGetComponent(out BoostView boostView))
            {
                _boostsActivator.Activate(boostView);

                _simpleDestroyService.Destroy(boostView);
            }
            else
            {
                if (obj.collider.TryGetComponent(out BallView ballView))
                {
                    if (!_ballsService.BallIsSticked(ballView))
                    {
                        _playerView
                            .transform
                            .DOMoveY(_initialY - 0.18f, 0.1f)
                            .SetLoops(2, LoopType.Yoyo);
                    }
                }
            }
        }
    }
}