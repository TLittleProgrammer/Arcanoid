using System;
using App.Scripts.Scenes.GameScene.Features.Boosts.General;
using App.Scripts.Scenes.GameScene.Features.Components;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Entities.View
{
    public class EntityView : MonoBehaviour, IEntityView
    {
        [SerializeField] private SpriteRenderer _mainSpriteRenderer;
        [SerializeField] private SpriteRenderer _onTopSpriteRenderer;
        [SerializeField] private BoxCollider2D _collider2D;

        public int GridPositionX { get; set; }
        public int GridPositionY { get; set; }
        public GameObject GameObject => gameObject;
        public event Action<IEntityView, Collision2D> Colliderable;

        private void OnCollisionEnter2D(Collision2D col)
        {
            Colliderable?.Invoke(this, col);
        }

        public Sprite MainSprite
        {
            get => _mainSpriteRenderer.sprite;
            set => _mainSpriteRenderer.sprite = value;
        }

        public Sprite OnTopSprite 
        {
            get => _onTopSpriteRenderer.sprite;
            set => _onTopSpriteRenderer.sprite = value;
        }

        public BoostTypeId BoostTypeId { get; set; }
        public int EntityId { get; set; }

        public Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }

        public Vector3 Scale
        {
            get => transform.localScale;
            set => transform.localScale = value;
        }

        public BoxCollider2D BoxCollider2D => _collider2D;


        public class Pool : MonoMemoryPool<EntityView>
        {
            
        }
    }
}