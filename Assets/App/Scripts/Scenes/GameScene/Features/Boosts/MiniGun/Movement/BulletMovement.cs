using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.Features.Boosts.MiniGun.Bullets;
using App.Scripts.Scenes.GameScene.Features.Settings;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.MiniGun.Movement
{
    public class BulletMovement : IBulletMovement
    {
        private readonly BoostsSettings _boostsSettings;
        private readonly List<BulletView> _bullets;

        public BulletMovement(BoostsSettings boostsSettings)
        {
            _boostsSettings = boostsSettings;
            _bullets = new();
        }

        public void InitializeBullet(BulletView bulletView)
        {
            bulletView.Rigidbody2D.velocity = Vector2.up * _boostsSettings.BulletSpeed;
            
            _bullets.Add(bulletView);
        }

        public void UpdateVelocityForAll(Vector2 velocity)
        {
            foreach (BulletView view in _bullets)
            {
                view.Rigidbody2D.velocity = velocity;
            }
        }

        public void RemoveBullet(BulletView bulletView)
        {
            _bullets.Remove(bulletView);
        }
    }
}