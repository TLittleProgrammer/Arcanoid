using System;
using App.Scripts.Scenes.GameScene.Components;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Ball
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class BallView : MonoBehaviour, IRigidablebody, ISpriteRenderable
    {
        public event Action<Collider2D> Collidered;
        
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
            Collidered?.Invoke(collision.collider);
        }
    }
}