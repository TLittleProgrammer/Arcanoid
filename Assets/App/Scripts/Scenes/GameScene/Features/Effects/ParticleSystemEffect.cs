using App.Scripts.Scenes.GameScene.Features.Components;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Effects
{
    [RequireComponent(typeof(ParticleSystem))]
    public abstract class ParticleSystemEffect : MonoBehaviour, IPositionable, IScalable<float>
    {
        [SerializeField] private ParticleSystem _particleSystem;

        private ParticleSystem.MainModule _particlesMainModule;
        
        protected void Awake()
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

        public void PlayEffect()
        {
            _particleSystem.Play();
        }

        public void StopEffect()
        {
            _particleSystem.Stop();
        }
    }
}