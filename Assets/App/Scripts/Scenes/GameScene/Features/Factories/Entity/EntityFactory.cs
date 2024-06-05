using App.Scripts.Scenes.GameScene.Features.Entities.AssetManagement;
using App.Scripts.Scenes.GameScene.Features.Entities.View;
using App.Scripts.Scenes.GameScene.Features.Entities.View.Collisions;
using App.Scripts.Scenes.GameScene.Features.Pools;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Factories.Entity
{
    public class EntityFactory : IFactory<string, IEntityView>
    {
        private readonly EntityProvider _entityProvider;
        private readonly EntityView.Pool _entityViewPool;
        private readonly IEntityCollisionService _entityCollisionService;

        public EntityFactory(
            EntityProvider entityProvider,
            EntityView.Pool entityViewPool,
            IEntityCollisionService entityCollisionService)
        {
            _entityProvider = entityProvider;
            _entityViewPool = entityViewPool;
            _entityCollisionService = entityCollisionService;
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
            
            _entityCollisionService.AddEntity(entityView);

            return entityView;
        }
    }
}