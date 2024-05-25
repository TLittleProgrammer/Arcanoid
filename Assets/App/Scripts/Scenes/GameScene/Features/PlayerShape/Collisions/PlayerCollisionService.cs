﻿using App.Scripts.Scenes.GameScene.Features.Boosts;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.PlayerShape.Collisions
{
    public class PlayerCollisionService
    {
        private readonly IBoostsActivator _boostsActivator;

        public PlayerCollisionService(
            PlayerView playerView,
            IBoostsActivator boostsActivator)
        {
            _boostsActivator = boostsActivator;
            playerView.Collided += OnCollided;
        }

        private void OnCollided(Collision2D obj)
        {
            if (obj.collider.TryGetComponent(out BoostView boostView))
            {
                _boostsActivator.Activate(boostView);
            }
        }
    }
}