using App.Scripts.Scenes.GameScene.Features.Effects;
using App.Scripts.Scenes.GameScene.Features.Entities;
using App.Scripts.Scenes.GameScene.Features.Entities.View;
using App.Scripts.Scenes.GameScene.Features.Pools;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Factories.CircleEffect
{
    public class CircleEffectFactory : IFactory<EntityView, CircleEffects>
    {
        private readonly CircleEffects.Pool _circleEffectPool;

        public CircleEffectFactory(CircleEffects.Pool circleEffectPool)
        {
            _circleEffectPool = circleEffectPool;
        }
        
        public CircleEffects Create(EntityView targetType)
        {
            CircleEffects effects = _circleEffectPool.Spawn();
            
            effects.Position = targetType.Position;
            effects.Scale    = targetType.Scale.x;
            effects.ScaleForSubParticles = effects.Scale / 10f;

            return effects;
        }
    }
}