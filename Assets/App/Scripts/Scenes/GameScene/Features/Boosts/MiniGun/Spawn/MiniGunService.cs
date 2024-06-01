using App.Scripts.Scenes.GameScene.Features.Settings;
using App.Scripts.Scenes.GameScene.Features.Time;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.MiniGun
{
    public class MiniGunService : IMiniGunService, ITickable
    {
        private readonly BoostsSettings _boostsSettings;
        private readonly ITimeProvider _timeProvider;
        private readonly ISpawnBulletService _spawnBulletService;
        private readonly IBulletMovement _bulletMovement;

        private float _currentTime;
        private bool _miniGunIsActive;

        public MiniGunService(
            BoostsSettings boostsSettings,
            ITimeProvider timeProvider,
            ISpawnBulletService spawnBulletService,
            IBulletMovement bulletMovement)
        {
            _boostsSettings = boostsSettings;
            _timeProvider = timeProvider;
            _spawnBulletService = spawnBulletService;
            _bulletMovement = bulletMovement;
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

        public void Tick()
        {
            if (!ActiveMiniGun || !IsActive)
                return;

            UpdateTime();
        }

        private void UpdateTime()
        {
            _currentTime += _timeProvider.DeltaTime;

            if (_currentTime >= _boostsSettings.BulletTimeOffset)
            {
                _currentTime = 0f;
                SpawnBullets();
            }
        }

        private void SpawnBullets()
        {
            _spawnBulletService.Spawn();
        }

        private void UpdateVelocityForAllBullets(Vector2 value)
        {
            _bulletMovement.UpdateVelocityForAll(value);
        }
    }
}