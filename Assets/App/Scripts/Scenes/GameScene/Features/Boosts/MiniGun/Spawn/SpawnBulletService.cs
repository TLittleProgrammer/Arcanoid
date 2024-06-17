using App.Scripts.Scenes.GameScene.Features.Boosts.MiniGun.Bullets;
using App.Scripts.Scenes.GameScene.Features.Boosts.MiniGun.Collision;
using App.Scripts.Scenes.GameScene.Features.Boosts.MiniGun.Movement;
using App.Scripts.Scenes.GameScene.Features.Boosts.MiniGun.PositionChecker;
using App.Scripts.Scenes.GameScene.Features.Entities.PlayerShape;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.MiniGun.Spawn
{
    public sealed class SpawnBulletService : ISpawnBulletService
    {
        private readonly IBulletPositionChecker _bulletPositionChecker;
        private readonly IBulletMovement _bulletMovement;
        private readonly IBulletCollisionService _bulletCollisionService;
        private readonly BulletView.Pool _bulletsPool;
        private readonly PlayerView _playerView;

        public SpawnBulletService(
            IBulletPositionChecker bulletPositionChecker,
            IBulletMovement bulletMovement,
            IBulletCollisionService bulletCollisionService,
            BulletView.Pool bulletsPool,
            PlayerView playerView)
        {
            _bulletPositionChecker = bulletPositionChecker;
            _bulletMovement = bulletMovement;
            _bulletCollisionService = bulletCollisionService;
            _bulletsPool = bulletsPool;
            _playerView = playerView;
        }
        
        public void Spawn()
        {
            foreach (Transform position in _playerView.BulletsInitialPositions)
            {
                SpawnBullet(position);
            }
        }

        private void SpawnBullet(Transform transform)
        {
            BulletView bulletView = _bulletsPool.Spawn();
            bulletView.Position = transform.position;
            
            _bulletMovement.InitializeBullet(bulletView);
            _bulletCollisionService.AddBullet(bulletView);
            _bulletPositionChecker.AddBullet(bulletView);
        }
    }
}