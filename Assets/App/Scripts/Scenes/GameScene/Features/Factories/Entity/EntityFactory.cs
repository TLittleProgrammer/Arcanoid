using App.Scripts.Scenes.GameScene.Features.Entities.AssetManagement;
using App.Scripts.Scenes.GameScene.Features.Entities.View;
using App.Scripts.Scenes.GameScene.Features.Pools;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Factories.Entity
{
    public class EntityFactory : IFactory<string, IEntityView>
    {
        private readonly EntityProvider _entityProvider;
        private readonly EntityView.Pool _entityViewPool;
        private readonly IPoolContainer _poolContainer;

        public EntityFactory(EntityProvider entityProvider, EntityView.Pool entityViewPool)
        {
            _entityProvider = entityProvider;
            _entityViewPool = entityViewPool;
        }

        public IEntityView Create(string targetType)
        {
            if (!_entityProvider.EntityStages.ContainsKey(targetType))
            {
                return null;
            }

            EntityStage entityStage = _entityProvider.EntityStages[targetType];
            EntityView entityView   = _entityViewPool.Spawn();

            entityView.MainSprite   = entityStage.Sprite;
            entityView.BoostTypeId = entityStage.BoostTypeId;

            return entityView;
        }
    }
}