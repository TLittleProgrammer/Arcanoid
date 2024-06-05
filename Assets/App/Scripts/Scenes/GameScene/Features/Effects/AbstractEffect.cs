using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Effects.ObjectPool
{
    public abstract class AbstractEffect : MonoBehaviour, IEffect, IPoolable
    {
        [SerializeField] private string _poolKey;
        [SerializeField] private ParticleSystem _particleSystem;

        public string PoolKey => _poolKey;

        public abstract void PlayEffect();
        public abstract void StopEffect();
    }
}