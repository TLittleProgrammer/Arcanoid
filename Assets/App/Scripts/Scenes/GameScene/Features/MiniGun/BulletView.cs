using System;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.MiniGun
{
    public class BulletView : MonoBehaviour
    {
        public Rigidbody2D Rigidbody2D;
        public event Action<BulletView, Collision2D> Collided;

        private void OnDisable()
        {
            Collided = null;
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            Collided?.Invoke(this, col);
        }

        public class Pool : MonoMemoryPool<BulletView>
        {
            
        }
    }
}