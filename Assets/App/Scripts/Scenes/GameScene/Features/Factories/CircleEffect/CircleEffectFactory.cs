using App.Scripts.Scenes.GameScene.Features.Entities;
using App.Scripts.Scenes.GameScene.Features.Entities.View;
using App.Scripts.Scenes.GameScene.Features.Pools;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Factories.CircleEffect
{
    public class CircleEffectFactory : IFactory<EntityView, Effects.CircleEffect>
    {
        private readonly IPoolContainer _poolContainer;

        public CircleEffectFactory(IPoolContainer poolContainer)
        {
            _poolContainer = poolContainer;
        }
        
        public Effects.CircleEffect Create(EntityView targetType)
        {
            Effects.CircleEffect effect = _poolContainer.GetItem<Effects.CircleEffect>(PoolTypeId.CircleEffect);

            effect.Position = targetType.Position;
            effect.Scale    = targetType.Scale.x;
            effect.ScaleForSubParticles = effect.Scale / 10f;

            return effect;
        }
    }
}