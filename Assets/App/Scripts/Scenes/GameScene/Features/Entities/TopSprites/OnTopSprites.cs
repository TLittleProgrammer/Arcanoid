using App.Scripts.Scenes.GameScene.Features.Components;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Entities.TopSprites
{
    public class OnTopSprites : MonoBehaviour, ISpriteRenderable
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        public SpriteRenderer SpriteRenderer => _spriteRenderer;

        public void SetSprite(Sprite sprite)
        {
            _spriteRenderer.sprite = sprite;
        }
        
        public class Pool : MonoMemoryPool<OnTopSprites>
        {
            
        }

        public class Factory : PlaceholderFactory<IEntityView, OnTopSprites>
        {
            
        }
    }
}