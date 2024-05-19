using App.Scripts.Scenes.GameScene.Entities;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Effects
{
    public class CircleEffect : ParticleSystemEffect<CircleEffect>
    {
        [SerializeField] private ParticleSystem _subParticleSystem;

        private ParticleSystem.MainModule _subParticlesMainModule;
        
        protected new void Awake()
        {
            base.Awake();
            _subParticlesMainModule = _subParticleSystem.main;
        }
        
        public float ScaleForSubParticles
        {
            set => _subParticlesMainModule.startSize = value;
        }
        
        public class Factory : PlaceholderFactory<EntityView, CircleEffect>
        {
            
        }
    }
}