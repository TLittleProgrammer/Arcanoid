using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.Features.Boosts.MiniGun.Bullets;
using App.Scripts.Scenes.GameScene.Features.Entities.PlayerShape;
using App.Scripts.Scenes.GameScene.Features.Settings;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.MiniGun.Movement
{
    public class BulletMovement : IBulletMovement
    {
        private readonly PlayerView _playerView;
        private readonly BoostsSettings _boostsSettings;
        private readonly List<BulletView> _bullets;
        
        private float _shapeWidth;
        private float _shapeHeight;
        private int _bulletCounter;
        
        public BulletMovement(PlayerView playerView, BoostsSettings boostsSettings)
        {
            _playerView = playerView;
            _boostsSettings = boostsSettings;
            _bullets = new();
            
            RecalculateSpawnPositions();
        }

        public void InitializeBullet(BulletView bulletView)
        {
            bulletView.Rigidbody2D.velocity = Vector2.up * _boostsSettings.BulletSpeed;
            bulletView.transform.position = GetBulletPosition(_shapeWidth * ChooseSpawnSide());
            
            _bullets.Add(bulletView);
        }

        private float ChooseSpawnSide()
        {
            _bulletCounter++;
            _bulletCounter %= 2;

            return _bulletCounter == 0 ? 1f : -1f;
        }

        public void UpdateVelocityForAll(Vector2 velocity)
        {
            foreach (BulletView view in _bullets)
            {
                view.Rigidbody2D.velocity = velocity;
            }
        }

        public void RecalculateSpawnPositions()
        {
            Bounds shapeBounds = _playerView.SpriteRenderer.bounds;
            _shapeWidth = shapeBounds.size.x / 2f;
            _shapeHeight = shapeBounds.size.y / 2f;
        }

        public void RemoveBullet(BulletView bulletView)
        {
            _bullets.Remove(bulletView);
        }

        private Vector3 GetBulletPosition(float offset)
        {
            Vector3 playerPosition = _playerView.transform.position;
            
            return new Vector3
            (
                playerPosition.x + offset, 
                playerPosition.y + _shapeHeight,
                0f
            );
        }
    }
}