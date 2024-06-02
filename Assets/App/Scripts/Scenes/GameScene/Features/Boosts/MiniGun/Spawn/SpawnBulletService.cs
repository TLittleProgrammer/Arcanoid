using App.Scripts.Scenes.GameScene.Features.Boosts.MiniGun.Bullets;
using App.Scripts.Scenes.GameScene.Features.Boosts.MiniGun.Collision;
using App.Scripts.Scenes.GameScene.Features.Boosts.MiniGun.Movement;
using App.Scripts.Scenes.GameScene.Features.Boosts.MiniGun.PositionChecker;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.MiniGun.Spawn
{
    public sealed class SpawnBulletService : ISpawnBulletService
    {
        private readonly IBulletPositionChecker _bulletPositionChecker;
        private readonly IBulletMovement _bulletMovement;
        private readonly IBulletCollisionService _bulletCollisionService;
        private readonly BulletView.Pool _bulletsPool;

        public SpawnBulletService(IBulletPositionChecker bulletPositionChecker, IBulletMovement bulletMovement, IBulletCollisionService bulletCollisionService, BulletView.Pool bulletsPool)
        {
            _bulletPositionChecker = bulletPositionChecker;
            _bulletMovement = bulletMovement;
            _bulletCollisionService = bulletCollisionService;
            _bulletsPool = bulletsPool;
        }
        
        public void Spawn()
        {
            SpawnBullet();
            SpawnBullet();
        }

        private void SpawnBullet()
        {
            BulletView bulletView = _bulletsPool.Spawn();
            
            _bulletMovement.InitializeBullet(bulletView);
            _bulletCollisionService.AddBullet(bulletView);
            _bulletPositionChecker.AddBullet(bulletView);
        }
    }
}