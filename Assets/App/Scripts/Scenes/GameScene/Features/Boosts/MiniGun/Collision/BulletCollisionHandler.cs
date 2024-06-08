using App.Scripts.External.ObjectPool;
using App.Scripts.Scenes.GameScene.Features.Boosts.MiniGun.Bullets;
using App.Scripts.Scenes.GameScene.Features.Constants;
using App.Scripts.Scenes.GameScene.Features.Effects;
using App.Scripts.Scenes.GameScene.Features.Entities.View;
using App.Scripts.Scenes.GameScene.Features.Shake;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.MiniGun.Collision
{
    public sealed class BulletCollisionHandler : IBulletCollisionHandler
    {
        private readonly IShakeService _shakeService;
        private readonly IKeyObjectPool<IEffect> _keyObjectPool;

        public BulletCollisionHandler(
            IBulletCollisionService bulletCollisionService,
            IShakeService shakeService,
            IKeyObjectPool<IEffect> keyObjectPool)
        {
            _shakeService = shakeService;
            _keyObjectPool = keyObjectPool;
            bulletCollisionService.BulletWasCollidired += OnBulletWasCollidered;
        }

        private void OnBulletWasCollidered(BulletView bulletView, EntityView entityView)
        {
            IEffect effect = _keyObjectPool.Spawn(PoolConstants.BulletEffectKey);
            effect.PlayEffect(bulletView.transform, entityView.transform);

            effect.Disabled += OnDisableEffect;
            
            _shakeService.Shake(entityView.transform);
        }

        private void OnDisableEffect(IEffect effect)
        {
            effect.Disabled -= OnDisableEffect;
            
            _keyObjectPool.Despawn(PoolConstants.BulletEffectKey, effect);
        }
    }
}