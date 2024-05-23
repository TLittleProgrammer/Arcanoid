using App.Scripts.Scenes.GameScene.Features.Entities;
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
        
        public Effects.CircleEffect Create(EntityView entityView)
        {
            Effects.CircleEffect effect = _poolContainer.GetItem<Effects.CircleEffect>(PoolTypeId.CircleEffect);

            effect.Position = entityView.Position;
            effect.Scale    = entityView.Scale.x;
            effect.ScaleForSubParticles = effect.Scale / 10f;

            return effect;
        }
    }
}