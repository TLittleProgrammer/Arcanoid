using System;
using App.Scripts.Scenes.GameScene.Features.Blocks;
using App.Scripts.Scenes.GameScene.Features.Components;
using App.Scripts.Scenes.GameScene.Features.Effects;
using App.Scripts.Scenes.GameScene.Features.Entities;
using App.Scripts.Scenes.GameScene.Features.Healthes;
using App.Scripts.Scenes.GameScene.Features.Levels.View;
using App.Scripts.Scenes.GameScene.Features.ScreenInfo;
using DG.Tweening;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Ball.Collision
{
    public class BallCollisionService : IBallCollisionService
    {
        private readonly BallView _ball;
        private readonly IHealthContainer _healthContainer;
        private readonly CircleEffect.Factory _circleEffectFactory;
        private readonly ILevelViewUpdater _levelViewUpdater;
        private readonly IScreenInfoProvider _screenInfoProvider;
        private readonly IBlockShakeService _blockShakeService;
        private readonly float _minBallYPosition;

        public BallCollisionService(
            BallView ball,
            IHealthContainer healthContainer,
            CircleEffect.Factory circleEffectFactory,
            ILevelViewUpdater levelViewUpdater,
            IScreenInfoProvider screenInfoProvider,
            IBlockShakeService blockShakeService)
        {
            _ball = ball;
            _healthContainer = healthContainer;
            _circleEffectFactory = circleEffectFactory;
            _levelViewUpdater = levelViewUpdater;
            _screenInfoProvider = screenInfoProvider;
            _blockShakeService = blockShakeService;

            _minBallYPosition = ball.SpriteRenderer.bounds.size.y -
                                _screenInfoProvider.HeightInWorld / 2f + 0.25f;

            _ball.Collidered += OnCollidered;
        }

        private void OnCollidered(Collider2D collider)
        {
            if (_ball.Position.y <= _minBallYPosition)
            {
                _healthContainer.UpdateHealth(-1);
            }
            else if (collider.TryGetComponent(out EntityView entityView))
            {
                PlayEffects(entityView);

                _blockShakeService.Shake(entityView);

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