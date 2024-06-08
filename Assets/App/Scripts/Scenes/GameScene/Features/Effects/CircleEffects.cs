using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Effects
{
    public class CircleEffects : ParticleSystemEffect
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

        public override void PlayEffect(Transform initialEntityTransform, Transform colliderEntityTransform)
        {
            var initialEntityScale = initialEntityTransform.localScale;
            
            Position = initialEntityTransform.position;
            Scale = initialEntityScale.x;
            ScaleForSubParticles = initialEntityScale.x / 10f;
            
            ParticleSystem.Play();
        }
    }
}