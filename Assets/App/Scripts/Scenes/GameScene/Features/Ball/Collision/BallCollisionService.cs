using App.Scripts.Scenes.GameScene.Components;
using App.Scripts.Scenes.GameScene.Effects;
using App.Scripts.Scenes.GameScene.Entities;
using App.Scripts.Scenes.GameScene.Healthes;
using App.Scripts.Scenes.GameScene.Levels.View;
using App.Scripts.Scenes.GameScene.ScreenInfo;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Ball.Collision
{
    public class BallCollisionService : IBallCollisionService
    {
        private readonly IRigidablebody _ball;
        private readonly IHealthContainer _healthContainer;
        private readonly CircleEffect.Factory _circleEffectFactory;
        private readonly ILevelViewUpdater _levelViewUpdater;
        private readonly IScreenInfoProvider _screenInfoProvider;
        private readonly float _minBallYPosition;

        public BallCollisionService(
            IRigidablebody ball,
            IHealthContainer healthContainer,
            CircleEffect.Factory circleEffectFactory,
            ILevelViewUpdater levelViewUpdater,
            IScreenInfoProvider screenInfoProvider)
        {
            _ball = ball;
            _healthContainer = healthContainer;
            _circleEffectFactory = circleEffectFactory;
            _levelViewUpdater = levelViewUpdater;
            _screenInfoProvider = screenInfoProvider;

            _minBallYPosition = (_ball as ISpriteRenderable).SpriteRenderer.bounds.size.y -
                                _screenInfoProvider.HeightInWorld / 2f;

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
                _levelViewUpdater.UpdateVisual(entityView);
            }
        }

        private void PlayEffects(EntityView entityView)
        {
            CircleEffect circleEffect = _circleEffectFactory.Create(entityView);
            circleEffect.PlayEffect();
        }
    }
}