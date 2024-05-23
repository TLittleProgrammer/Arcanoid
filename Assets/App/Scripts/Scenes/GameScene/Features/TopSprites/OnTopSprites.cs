using App.Scripts.Scenes.GameScene.Components;
using App.Scripts.Scenes.GameScene.Entities;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene.TopSprites
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