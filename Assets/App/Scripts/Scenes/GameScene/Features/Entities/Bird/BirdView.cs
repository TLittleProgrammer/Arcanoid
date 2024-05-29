using System;
using App.Scripts.External.Components;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Entities.Bird
{
    public class BirdView : MonoBehaviour, ITransformable
    {
        public SpriteRenderer SpriteRenderer;
        public Transform Transform => transform;

        public event Action<BirdView> Collidered; 

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Collidered?.Invoke(this);
        }

        public class Pool : MonoMemoryPool<BirdView>
        {
            
        }

        public class Factory : PlaceholderFactory<BirdView>
        {
            
        }
    }
}