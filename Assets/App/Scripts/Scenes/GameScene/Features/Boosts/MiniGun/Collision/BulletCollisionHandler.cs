using App.Scripts.Scenes.GameScene.Features.Boosts.MiniGun.Bullets;
using App.Scripts.Scenes.GameScene.Features.Entities.View;
using App.Scripts.Scenes.GameScene.Features.Shake;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.MiniGun.Collision
{
    public sealed class BulletCollisionHandler : IBulletCollisionHandler
    {
        private readonly IShakeService _shakeService;
        private readonly BulletEffectView.Pool _bulletEffectViewPool;

        public BulletCollisionHandler(
            IBulletCollisionService bulletCollisionService,
            IShakeService shakeService,
            BulletEffectView.Pool bulletEffectViewPool)
        {
            _shakeService = shakeService;
            _bulletEffectViewPool = bulletEffectViewPool;
            bulletCollisionService.BulletWasCollidired += OnBulletWasCollidered;
        }

        private void OnBulletWasCollidered(BulletView bulletView, EntityView entityView)
        {
            BulletEffectView bulletEffectView = _bulletEffectViewPool.Spawn();
            bulletEffectView.transform.position = bulletView.transform.position + Vector3.down * 0.25f;
            bulletEffectView.ParticleSystem.Play();
                
            _shakeService.Shake(entityView.transform);
        }
    }
}