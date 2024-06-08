using App.Scripts.External.ObjectPool;
using App.Scripts.Scenes.GameScene.Features.Constants;
using App.Scripts.Scenes.GameScene.Features.Effects;
using App.Scripts.Scenes.GameScene.Features.Entities.View;
using App.Scripts.Scenes.GameScene.Features.Entities.Walls;
using App.Scripts.Scenes.GameScene.Features.Levels.General.View;
using App.Scripts.Scenes.GameScene.Features.Shake;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Entities.Ball.Collision
{
    public class BallCollisionService : IBallCollisionService
    {
        private readonly BallView _ball;
        private readonly ILevelViewUpdater _levelViewUpdater;
        private readonly IShakeService _shakeService;
        private readonly IKeyObjectPool<IEffect> _keyObjectPool;
        private readonly float _minBallYPosition;

        public BallCollisionService(
            BallView ball,
            ILevelViewUpdater levelViewUpdater,
            IShakeService shakeService,
            IKeyObjectPool<IEffect> keyObjectPool)
        {
            _ball = ball;
            _levelViewUpdater = levelViewUpdater;
            _shakeService = shakeService;
            _keyObjectPool = keyObjectPool;

            _ball.Collidered += OnCollidered;
        }

        private void OnCollidered(BallView view, Collider2D collider)
        {
            if (collider.TryGetComponent(out EntityView entityView))
            {
                _shakeService.Shake(entityView.transform);
                _levelViewUpdater.UpdateVisual(entityView, 1);
            }
            else if (collider.TryGetComponent(out WallView wallView))
            {
                PlayPlazmaEffect(wallView.gameObject.transform);
            }
        }

        private void PlayPlazmaEffect(Transform wallTransform)
        {
            IEffect effect = _keyObjectPool.Spawn(PoolConstants.PlazmaEffectKey);
            
            effect.PlayEffect(_ball.transform, wallTransform);

            effect.Disabled += OnPlazmaEffectDisabled;
        }

        private void OnPlazmaEffectDisabled(IEffect effect)
        {
            _keyObjectPool.Despawn(PoolConstants.PlazmaEffectKey, effect);
        }
    }
}