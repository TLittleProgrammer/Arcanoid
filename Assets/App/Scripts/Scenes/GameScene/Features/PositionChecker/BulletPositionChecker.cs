using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.Features.Boosts.MiniGun;
using App.Scripts.Scenes.GameScene.Features.ScreenInfo;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.PositionChecker
{
    public sealed class BulletPositionChecker : IBulletPositionChecker, ITickable
    {
        private readonly BulletView.Pool _bulletsPool;
        private readonly float _maxHeight;
        private List<BulletView> _bullets = new();
        
        public BulletPositionChecker(IScreenInfoProvider screenInfoProvider, BulletView.Pool bulletsPool)
        {
            _bulletsPool = bulletsPool;
            _maxHeight = screenInfoProvider.HeightInWorld / 2f + 1f;
        }

        public void Tick()
        {
            for (int i = 0; i < _bullets.Count; i++)
            {
                BulletView bullet = _bullets[i];

                if (bullet.transform.position.y >= _maxHeight)
                {
                    _bullets.Remove(bullet);
                    _bulletsPool.Despawn(bullet);
                    i--;
                }
            }
        }

        public IEnumerable<BulletView> GetAll()
        {
            return _bullets;
        }

        public void AddBullet(BulletView bulletView)
        {
            _bullets.Add(bulletView);
        }
        
        public void RemoveBullet(BulletView bulletView)
        {
            _bullets.Remove(bulletView);
        }

        public void Restart()
        {
            foreach (BulletView bulletView in _bullets)
            {
                _bulletsPool.Despawn(bulletView);
            }
            
            _bullets.Clear();
        }
    }
}