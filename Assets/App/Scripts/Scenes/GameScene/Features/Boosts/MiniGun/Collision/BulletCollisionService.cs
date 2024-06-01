using System;
using App.Scripts.Scenes.GameScene.Features.Entities.View;
using App.Scripts.Scenes.GameScene.Features.Levels.General.View;
using App.Scripts.Scenes.GameScene.Features.PositionChecker;
using App.Scripts.Scenes.GameScene.Features.Settings;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.MiniGun
{
    public sealed class BulletCollisionService : IBulletCollisionService
    {
        private readonly BulletView.Pool _bulletViewPool;
        private readonly BoostsSettings _boostsSettings;
        private readonly ILevelViewUpdater _levelViewUpdater;
        private readonly IBulletPositionChecker _bulletPositionChecker;
        private readonly IBulletMovement _bulletMovement;

        public BulletCollisionService(
            BulletView.Pool bulletViewPool,
            BoostsSettings boostsSettings,
            ILevelViewUpdater levelViewUpdater,
            IBulletPositionChecker bulletPositionChecker,
            IBulletMovement bulletMovement)
        {
            _bulletViewPool = bulletViewPool;
            _boostsSettings = boostsSettings;
            _levelViewUpdater = levelViewUpdater;
            _bulletPositionChecker = bulletPositionChecker;
            _bulletMovement = bulletMovement;
        }

        public event Action<BulletView, EntityView> BulletWasCollidired;

        public void AddBullet(BulletView bulletView)
        {
            bulletView.Collided += OnBulletCollided;
        }

        public void OnBulletCollided(BulletView bulletView, Collision2D collider)
        {
            if (collider.transform.TryGetComponent(out EntityView entity))
            {
                RemoveFromAll(bulletView);

                BulletWasCollidired?.Invoke(bulletView, entity);
                
                _levelViewUpdater.UpdateVisual(entity, _boostsSettings.BulletDamage);
            }
        }

        private void RemoveFromAll(BulletView bulletView)
        {
            _bulletPositionChecker.RemoveBullet(bulletView);
            _bulletMovement.RemoveBullet(bulletView);
            _bulletViewPool.Despawn(bulletView);
        }
    }
}