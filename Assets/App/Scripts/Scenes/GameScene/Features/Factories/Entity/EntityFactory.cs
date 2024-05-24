using App.Scripts.Scenes.GameScene.Features.Entities;
using App.Scripts.Scenes.GameScene.Features.Levels.AssetManagement;
using App.Scripts.Scenes.GameScene.Features.Pools;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Factories.Entity
{
    public class EntityFactory : IFactory<string, IEntityView>
    {
        private readonly EntityProvider _entityProvider;
        private readonly IPoolContainer _poolContainer;

        public EntityFactory(EntityProvider entityProvider, IPoolContainer poolContainer)
        {
            _entityProvider = entityProvider;
            _poolContainer = poolContainer;
        }

        public IEntityView Create(string targetType)
        {
            if (!_entityProvider.EntityStages.ContainsKey(targetType))
            {
                return null;
            }

            EntityStage entityStage = _entityProvider.EntityStages[targetType];
            EntityView entityView   = _poolContainer.GetItem<EntityView>(PoolTypeId.EntityView);

            entityView.MainSprite   = entityStage.Sprite;
            entityView.BoostTypeId = entityStage.BoostTypeId;

            return entityView;
        }
    }
}