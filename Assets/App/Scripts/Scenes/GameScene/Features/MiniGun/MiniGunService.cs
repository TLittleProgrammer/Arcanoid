using App.Scripts.Scenes.GameScene.Features.Blocks;
using App.Scripts.Scenes.GameScene.Features.Entities;
using App.Scripts.Scenes.GameScene.Features.Levels.View;
using App.Scripts.Scenes.GameScene.Features.PlayerShape;
using App.Scripts.Scenes.GameScene.Features.PositionChecker;
using App.Scripts.Scenes.GameScene.Features.Settings;
using App.Scripts.Scenes.GameScene.Features.Time;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.MiniGun
{
    public class MiniGunService : IMiniGunService, ITickable
    {
        private readonly PlayerView _playerView;
        private readonly BulletView.Pool _bulletsPool;
        private readonly BoostsSettings _boostsSettings;
        private readonly ITimeProvider _timeProvider;
        private readonly IBulletPositionChecker _bulletPositionChecker;
        private readonly ILevelViewUpdater _levelViewUpdater;
        private readonly IShakeService _shakeService;
        private readonly BulletEffectView.Pool _bulletEffectsPool;
        private float _shapeWidth;
        private float _shapeHeight;
        private float _currentTime = 0f;
        private bool _miniGunIsActive = false;

        public MiniGunService(
            PlayerView playerView,
            BulletView.Pool bulletsPool,
            BoostsSettings boostsSettings,
            ITimeProvider timeProvider,
            IBulletPositionChecker bulletPositionChecker,
            ILevelViewUpdater levelViewUpdater,
            IShakeService shakeService,
            BulletEffectView.Pool bulletEffectsPool)
        {
            _playerView = playerView;
            _bulletsPool = bulletsPool;
            _boostsSettings = boostsSettings;
            _timeProvider = timeProvider;
            _bulletPositionChecker = bulletPositionChecker;
            _levelViewUpdater = levelViewUpdater;
            _shakeService = shakeService;
            _bulletEffectsPool = bulletEffectsPool;

            RecalculateSpawnPositions();
        }

        public bool ActiveMiniGun { get; set; }

        public bool IsActive {
            
            get => _miniGunIsActive;
            set
            {
                _miniGunIsActive = value;

                if (_miniGunIsActive)
                {
                    UpdateVelocityForAllBullets(Vector2.up * _boostsSettings.BulletSpeed);
                }
                else
                {
                    UpdateVelocityForAllBullets(Vector2.zero);
                }
            }
        }

        public void RecalculateSpawnPositions()
        {
            Bounds shapeBounds = _playerView.SpriteRenderer.bounds;
            _shapeWidth = shapeBounds.size.x / 2f;
            _shapeHeight = shapeBounds.size.y / 2f;
        }

        public void Tick()
        {
            if (!ActiveMiniGun || !IsActive)
                return;

            _currentTime += _timeProvider.DeltaTime;

            if (_currentTime >= _boostsSettings.BulletTimeOffset)
            {
                _currentTime = 0f;
                SpawnBullets();
            }
        }

        private void SpawnBullets()
        {
            BulletView firstBullet = _bulletsPool.Spawn();
            BulletView secondBullet = _bulletsPool.Spawn();
            
            firstBullet.Rigidbody2D.velocity = Vector2.up * _boostsSettings.BulletSpeed;
            secondBullet.Rigidbody2D.velocity = Vector2.up * _boostsSettings.BulletSpeed;

            firstBullet.transform.position = GetBulletPosition(-_shapeWidth);
            secondBullet.transform.position = GetBulletPosition(_shapeWidth);

            firstBullet.Collided += OnBulletCollided;
            secondBullet.Collided += OnBulletCollided;
            
            _bulletPositionChecker.AddBullet(firstBullet);
            _bulletPositionChecker.AddBullet(secondBullet);
        }

        private void UpdateVelocityForAllBullets(Vector2 value)
        {
            foreach (BulletView view in _bulletPositionChecker.GetAll())
            {
                view.Rigidbody2D.velocity = value;
            }
        }

        private void OnBulletCollided(BulletView bulletView, Collision2D collider)
        {
            if (collider.transform.TryGetComponent(out EntityView entity))
            {
                _bulletPositionChecker.RemoveBullet(bulletView);
                _bulletsPool.Despawn(bulletView);

                BulletEffectView bulletEffectView = _bulletEffectsPool.Spawn();
                bulletEffectView.transform.position = bulletView.transform.position + Vector3.down * 0.25f;
                bulletEffectView.ParticleSystem.Play();
                
                _shakeService.Shake(entity.transform);
                _levelViewUpdater.UpdateVisual(entity, _boostsSettings.BulletDamage);
            }
        }

        private Vector3 GetBulletPosition(float offset)
        {
            return new Vector3
                (
                    _playerView.transform.position.x + offset, 
                    _playerView.transform.position.y + _shapeHeight,
                    0f
                );
        }
    }
}