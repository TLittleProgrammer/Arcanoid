using System;
using System.Collections.Generic;
using System.Linq;
using App.Scripts.Scenes.GameScene.Features.Boosts.MiniGun.Bullets;
using App.Scripts.Scenes.GameScene.Features.Components;
using App.Scripts.Scenes.GameScene.Features.Entities.Ball.PositionChecker;
using App.Scripts.Scenes.GameScene.Features.PositionCheckers;
using App.Scripts.Scenes.GameScene.Features.ScreenInfo;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.MiniGun.PositionChecker
{
    public sealed class BulletPositionChecker : IBulletPositionChecker, ITickable
    {
        private readonly BulletView.Pool _bulletsPool;
        private readonly List<IPositionChecker> _positionCheckers = new();
        private readonly float _maxHeight;
        
        private List<BulletView> _bullets = new();
        
        public BulletPositionChecker(
            IScreenInfoProvider screenInfoProvider,
            BulletView.Pool bulletsPool)
        {
            _bulletsPool = bulletsPool;
            _maxHeight = screenInfoProvider.HeightInWorld / 2f;
        }

        public void Tick()
        {
            for (int i = 0; i < _positionCheckers.Count; i++)
            {
                _positionCheckers[i].Tick();
            }
        }

        public IEnumerable<BulletView> GetAll()
        {
            return _bullets;
        }

        public void AddBullet(BulletView bulletView)
        {
            _bullets.Add(bulletView);

            AddPositionChecker(bulletView);
        }

        public void RemoveBullet(BulletView bulletView)
        {
            _bullets.Remove(bulletView);

            var result = _positionCheckers.First(x => x.Positionable.Equals(bulletView));
            _positionCheckers.Remove(result);
        }

        public void Restart()
        {
            foreach (BulletView bulletView in _bullets)
            {
                _bulletsPool.Despawn(bulletView);
            }
            
            _positionCheckers.Clear();
            _bullets.Clear();
        }

        private void AddPositionChecker(BulletView bulletView)
        {
            var condition = CreateCondition();
            var positionChecker = new VerticalPositionChecker(condition, bulletView, _maxHeight);

            positionChecker.WentAbroad += RemoveBullet;
            
            _positionCheckers.Add(positionChecker);
        }

        private void RemoveBullet(IPositionable positionable, IPositionChecker positionChecker)
        {
            _positionCheckers.Remove(positionChecker);

            BulletView bulletView = _bullets.First(x => x.Position.Equals(positionable.Position));
            if (!_bulletsPool.InactiveItems.Contains(bulletView))
            {
                _bulletsPool.Despawn(bulletView);
            }
        }

        private Func<IPositionable,float,bool> CreateCondition()
        {
            return (positionable, maxBorderPosition) => positionable.Position.y >= maxBorderPosition;
        }
    }
}