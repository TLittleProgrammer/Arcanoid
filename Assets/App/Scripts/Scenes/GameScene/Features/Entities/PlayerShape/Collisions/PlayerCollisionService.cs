using App.Scripts.Scenes.GameScene.Features.Boosts.General;
using App.Scripts.Scenes.GameScene.Features.Entities.EntityDestroyer.Helpers;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Entities.PlayerShape.Collisions
{
    public class PlayerCollisionService
    {
        private readonly IBoostsActivator _boostsActivator;
        private readonly SimpleDestroyService _simpleDestroyService;

        public PlayerCollisionService(
            PlayerView playerView,
            IBoostsActivator boostsActivator,
            SimpleDestroyService simpleDestroyService)
        {
            _boostsActivator = boostsActivator;
            _simpleDestroyService = simpleDestroyService;
            playerView.Collided += OnCollided;
        }

        private void OnCollided(Collision2D obj)
        {
            if (obj.collider.TryGetComponent(out BoostView boostView))
            {
                _boostsActivator.Activate(boostView);

                _simpleDestroyService.Destroy(boostView);
            }
        }
    }
}