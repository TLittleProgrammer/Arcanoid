using System;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.MiniGun.Bullets
{
    public class BulletView : MonoBehaviour
    {
        public Rigidbody2D Rigidbody2D;
        public event Action<BulletView, Collision2D> Collided;
        public event Action<BulletView, Collider2D> Triggered;

        private void OnDisable()
        {
            Collided = null;
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            Collided?.Invoke(this, col);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            Triggered?.Invoke(this, col);
        }

        public class Pool : MonoMemoryPool<BulletView>
        {
            
        }
    }
}