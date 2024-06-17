using System;
using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.Features.Components;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Entities.PlayerShape
{
    [RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
    public sealed class PlayerView : MonoBehaviour, IPositionable, ISpriteRenderable, IBoxColliderable2D
    {
        public List<Transform> BulletsInitialPositions;
        
        private SpriteRenderer _spriteRenderer;
        private BoxCollider2D _collider2D;

        public event Action<Collision2D> Collided;
        
        public Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }

        public SpriteRenderer SpriteRenderer => _spriteRenderer ??= GetComponent<SpriteRenderer>();
        public BoxCollider2D BoxCollider2D => _collider2D ??= GetComponent<BoxCollider2D>();

        private void OnCollisionEnter2D(Collision2D col)
        {
            Collided?.Invoke(col);
        }
    }
}