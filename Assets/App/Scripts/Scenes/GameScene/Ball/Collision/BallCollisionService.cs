using App.Scripts.Scenes.GameScene.Components;
using App.Scripts.Scenes.GameScene.Effects;
using App.Scripts.Scenes.GameScene.Entities;
using App.Scripts.Scenes.GameScene.Factories.CircleEffect;
using App.Scripts.Scenes.GameScene.Healthes;
using App.Scripts.Scenes.GameScene.Levels.View;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Ball.Collision
{
    public class BallCollisionService : IBallCollisionService
    {
        private readonly IRigidablebody _ball;
        private readonly IPositionable _playerView;
        private readonly IHealthContainer _healthContainer;
        private readonly CircleEffect.Factory _circleEffectFactory;
        private readonly ILevelViewUpdater _levelViewUpdater;

        public BallCollisionService(
            IRigidablebody ball,
            IPositionable playerView,
            IHealthContainer healthContainer,
            CircleEffect.Factory circleEffectFactory,
            ILevelViewUpdater levelViewUpdater)
        {
            _ball = ball;
            _playerView = playerView;
            _healthContainer = healthContainer;
            _circleEffectFactory = circleEffectFactory;
            _levelViewUpdater = levelViewUpdater;

            _ball.Collidered += OnCollidered;
        }

        private void OnCollidered(Collider2D collider)
        {
            if (_ball.Position.y < _playerView.Position.y)
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