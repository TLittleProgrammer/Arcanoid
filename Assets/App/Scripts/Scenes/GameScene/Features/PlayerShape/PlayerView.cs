using App.Scripts.Scenes.GameScene.Features.Components;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.PlayerShape
{
    [RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
    public sealed class PlayerView : MonoBehaviour, IPositionable, ISpriteRenderable, IBoxColliderable2D
    {
        private SpriteRenderer _spriteRenderer;
        private BoxCollider2D _collider2D;
        
        public Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }

        public SpriteRenderer SpriteRenderer => _spriteRenderer ??= GetComponent<SpriteRenderer>();
        public BoxCollider2D BoxCollider2D => _collider2D ??= GetComponent<BoxCollider2D>();
    }
}