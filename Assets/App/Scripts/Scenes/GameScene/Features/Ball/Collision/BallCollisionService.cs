﻿using App.Scripts.Scenes.GameScene.Features.Blocks;
using App.Scripts.Scenes.GameScene.Features.Effects;
using App.Scripts.Scenes.GameScene.Features.Entities;
using App.Scripts.Scenes.GameScene.Features.Levels.View;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Ball.Collision
{
    public class BallCollisionService : IBallCollisionService
    {
        private readonly BallView _ball;
        private readonly CircleEffect.Factory _circleEffectFactory;
        private readonly ILevelViewUpdater _levelViewUpdater;
        private readonly IBlockShakeService _blockShakeService;
        private readonly float _minBallYPosition;

        public BallCollisionService(
            BallView ball,
            CircleEffect.Factory circleEffectFactory,
            ILevelViewUpdater levelViewUpdater,
            IBlockShakeService blockShakeService)
        {
            _ball = ball;
            _circleEffectFactory = circleEffectFactory;
            _levelViewUpdater = levelViewUpdater;
            _blockShakeService = blockShakeService;


            _ball.Collidered += OnCollidered;
        }

        private void OnCollidered(Collider2D collider)
        {
            if (collider.TryGetComponent(out EntityView entityView))
            {
                PlayEffects(entityView);

                _blockShakeService.Shake(entityView.transform);
                _levelViewUpdater.UpdateVisual(entityView, 1);
            }
        }

        private void PlayEffects(EntityView entityView)
        {
            CircleEffect circleEffect = _circleEffectFactory.Create(entityView);
            circleEffect.PlayEffect();
        }
    }
}