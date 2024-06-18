using App.Scripts.Scenes.GameScene.Features.Entities.View;
using App.Scripts.Scenes.GameScene.Features.Settings;
using Codice.CM.SEIDInfo;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Effects
{
    public class MiniParticlesEffect : ParticleSystemEffect
    {
        private MiniSettingsParticleMapping _colorMapping;

        [Inject]
        private void Construct(MiniSettingsParticleMapping mapping)
        {
            _colorMapping = mapping;
        }
        
        public override void PlayEffect(Transform initialEntityTransform, Transform colliderEntityTransform)
        {
            var initialEntityScale = initialEntityTransform.localScale;
            
            Position = initialEntityTransform.position;
            Scale = initialEntityScale.x / 10f;

            var particleSystemShape = ParticleSystem.shape;
            particleSystemShape.scale = new(initialEntityScale.x, particleSystemShape.scale.y, particleSystemShape.scale.z);

            if (initialEntityTransform.TryGetComponent(out EntityView entityView))
            {
                Color color = Color.white;

                if (_colorMapping.Mapping.ContainsKey(entityView.EntityId))
                {
                    color = _colorMapping.Mapping[entityView.EntityId];
                }

                var particleSystemMain = ParticleSystem.main;
                particleSystemMain.startColor = color;
            }
            
            ParticleSystem.Play();
        }
    }
}