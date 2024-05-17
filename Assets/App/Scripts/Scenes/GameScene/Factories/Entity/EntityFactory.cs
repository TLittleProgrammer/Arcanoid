﻿using App.Scripts.Scenes.GameScene.Entities;
using App.Scripts.Scenes.GameScene.Levels.AssetManagement;
using App.Scripts.Scenes.GameScene.Pools;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Factories.Entity
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

        public IEntityView Create(string key)
        {
            if (!_entityProvider.EntityStages.ContainsKey(key))
            {
                return null;
            }

            EntityStage entityStage = _entityProvider.EntityStages[key];
            EntityView entityView   = _poolContainer.GetItem<EntityView>(PoolTypeId.EntityView);

            entityView.MainSprite   = entityStage.Sprite;

            return entityView;
        }
    }
}