using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.Effects;
using App.Scripts.Scenes.GameScene.Entities;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Pools
{
    public sealed class PoolContainer : IPoolContainer
    {
        private readonly EntityView.Pool _entityViewPool;
        private readonly IEffect<CircleEffect>.Pool _circleEffect;

        private List<MonoBehaviour> _entitySpawned = new();
        private List<MonoBehaviour> _circleEffectSpawned = new();

        public PoolContainer(EntityView.Pool entityViewPool, IEffect<CircleEffect>.Pool circleEffect)
        {
            _circleEffect = circleEffect;
            _entityViewPool = entityViewPool;
        }

        public TItem GetItem<TItem>(PoolTypeId poolTypeId) where TItem : MonoBehaviour
        {
            TItem item = poolTypeId switch
            {
                PoolTypeId.EntityView   => _entityViewPool.Spawn().GetComponent<TItem>(),
                PoolTypeId.CircleEffect => _circleEffect.Spawn().GetComponent<TItem>(),
                
                _ => null
            };

            return item;
        }

        public void RemoveItem<TItem>(PoolTypeId poolTypeId, TItem item) where TItem : MonoBehaviour
        {
            switch (poolTypeId)
            {
                case PoolTypeId.EntityView:   _entityViewPool.Despawn(item as EntityView); break;
                case PoolTypeId.CircleEffect: _circleEffect.Despawn(item as CircleEffect); break;
            }
        }

        public void Restart()
        {
            foreach (MonoBehaviour entity in _entitySpawned)
            {
                _entityViewPool.Despawn(entity as EntityView);
            }
            
            foreach (MonoBehaviour circle in _circleEffectSpawned)
            {
                _circleEffect.Despawn(circle as CircleEffect);
            }
        }
    }
}