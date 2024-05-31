using App.Scripts.Scenes.GameScene.Features.Boosts.General;
using App.Scripts.Scenes.GameScene.Features.Effects;
using App.Scripts.Scenes.GameScene.Features.Entities.TopSprites;
using App.Scripts.Scenes.GameScene.Features.Entities.View;
using App.Scripts.Scenes.GameScene.Features.Healthes.View;

namespace App.Scripts.Scenes.GameScene.Features.Pools
{
    public sealed class PoolContainer : IPoolContainer
    {
        private readonly EntityView.Pool _entityViewPool;
        private readonly CircleEffects.Pool _circleEffectPool;
        private readonly HealthPointView.Pool _healthPointPool;
        private readonly OnTopSprites.Pool _onTopSpritesPool;
        private readonly BoostView.Pool _boostsViewPool;


        public PoolContainer(
            EntityView.Pool entityViewPool,
            CircleEffects.Pool circleEffectPool,
            HealthPointView.Pool healthPointPool,
            OnTopSprites.Pool onTopSpritesPool,
            BoostView.Pool boostsViewPool)
        {
            _circleEffectPool = circleEffectPool;
            _healthPointPool = healthPointPool;
            _onTopSpritesPool = onTopSpritesPool;
            _boostsViewPool = boostsViewPool;
            _entityViewPool = entityViewPool;
        }

        public void Restart()
        {
            _entityViewPool.Clear();
            _circleEffectPool.Clear();
            _healthPointPool.Clear();
            _onTopSpritesPool.Clear();
            _boostsViewPool.Clear();
        }
    }
}