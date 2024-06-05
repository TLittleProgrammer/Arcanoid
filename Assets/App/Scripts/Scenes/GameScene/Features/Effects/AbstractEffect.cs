using System;
using App.Scripts.Scenes.GameScene.Features.Components;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Effects.ObjectPool
{
    public abstract class AbstractEffect : MonoBehaviour, IEffect, IPoolable, IPositionable, IScalable<float>
    {
        [SerializeField] protected string PoolabeKey;
        [SerializeField] protected ParticleSystem ParticleSystem;

        public string PoolKey => PoolabeKey;
        public event Action<AbstractEffect> Disabled;

        private void OnDisable()
        {
            Disabled?.Invoke(this);
        }

        public abstract void PlayEffect();
        public abstract void StopEffect();
        
        public Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }
        
        public abstract float Scale { get; set; }
    }
}