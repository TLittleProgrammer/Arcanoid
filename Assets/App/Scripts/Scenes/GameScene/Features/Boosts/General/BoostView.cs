using App.Scripts.Scenes.GameScene.Features.Boosts.General.Interfaces;
using App.Scripts.Scenes.GameScene.Features.Entities;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.General
{
    public class BoostView : MonoBehaviour, IBoostView
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        public Transform Transform => transform;
        public SpriteRenderer SpriteRenderer => _spriteRenderer;

        public BoostTypeId BoostTypeId { get; set; }

        public class Pool : MonoMemoryPool<BoostView>
        {
            
        }

        public class Factory : PlaceholderFactory<BoostTypeId, BoostView>
        {
        }
    }
}