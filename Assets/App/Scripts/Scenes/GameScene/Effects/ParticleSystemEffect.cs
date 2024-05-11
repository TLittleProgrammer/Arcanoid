using App.Scripts.Scenes.GameScene.Components;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Effects
{
    [RequireComponent(typeof(ParticleSystem))]
    public abstract class ParticleSystemEffect<TEffect> : MonoBehaviour, IEffect<TEffect>, ITransformable, IScalable<float> where TEffect : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleSystem;

        private ParticleSystem.MainModule _particlesMainModule;
        
        private void Awake()
        {
            _particlesMainModule = _particleSystem.main;
        }

        public Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }

        public float Scale
        {
            get => _particlesMainModule.startSize.constant;
            set => _particlesMainModule.startSize = value;
        }

        public virtual void PlayEffect()
        {
            _particleSystem.Play();
        }

        public virtual void StopEffect()
        {
            _particleSystem.Stop();
        }
    }
}