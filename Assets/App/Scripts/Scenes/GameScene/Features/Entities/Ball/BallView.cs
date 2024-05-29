using System;
using App.Scripts.Scenes.GameScene.Features.Components;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Entities.Ball
{
    [RequireComponent(typeof(SpriteRenderer), typeof(Rigidbody2D))]
    public class BallView : MonoBehaviour, IRigidablebody, ISpriteRenderable
    {
        public event Action<BallView, Collider2D> Collidered;

        public TrailRenderer TrailRenderer;
        public GameObject RedBall;
        public CircleCollider2D Collider2D;
        
        private SpriteRenderer _spriteRenderer;
        private Rigidbody2D _rigidbody2D;
        
        public Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }

        public SpriteRenderer SpriteRenderer => _spriteRenderer ??= GetComponent<SpriteRenderer>();
        public Rigidbody2D Rigidbody2D => _rigidbody2D ??= GetComponent<Rigidbody2D>();

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Collidered?.Invoke(this, collision.collider);
        }

        public class Pool : MonoMemoryPool<BallView>
        {
            
        }

        public class Factory : PlaceholderFactory<BallView>
        {
        }
    }
}