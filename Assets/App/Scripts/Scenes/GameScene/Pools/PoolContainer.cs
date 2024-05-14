using System.Collections.Generic;
using App.Scripts.General.Popup;
using App.Scripts.Scenes.GameScene.Effects;
using App.Scripts.Scenes.GameScene.Entities;
using App.Scripts.Scenes.GameScene.Healthes.View;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Pools
{
    public sealed class PoolContainer : IPoolContainer
    {
        private readonly EntityView.Pool _entityViewPool;
        private readonly IEffect<CircleEffect>.Pool _circleEffect;
        private readonly HealthPointView.Pool _healthPointPool;

        private List<MonoBehaviour> _entitySpawned = new();
        private List<MonoBehaviour> _circleEffectSpawned = new();
        private List<MonoBehaviour> _healthPointSpawned = new();

        public PoolContainer(EntityView.Pool entityViewPool, IEffect<CircleEffect>.Pool circleEffect, HealthPointView.Pool healthPointPool)
        {
            _circleEffect = circleEffect;
            _healthPointPool = healthPointPool;
            _entityViewPool = entityViewPool;
        }

        public TItem GetItem<TItem>(PoolTypeId poolTypeId) where TItem : MonoBehaviour
        {
            TItem item = poolTypeId switch
            {
                PoolTypeId.EntityView      => _entityViewPool.Spawn().GetComponent<TItem>(),
                PoolTypeId.CircleEffect    => _circleEffect.Spawn().GetComponent<TItem>(),
                PoolTypeId.HealthPointView => _healthPointPool.Spawn().GetComponent<TItem>(),
                
                _ => null
            };

            AddItemToList(item, poolTypeId);

            return item;
        }

        public void RemoveItem<TItem>(PoolTypeId poolTypeId, TItem item) where TItem : MonoBehaviour
        {
            switch (poolTypeId)
            {
                case PoolTypeId.EntityView:
                {
                    _entityViewPool.Despawn(item as EntityView);
                    _entitySpawned.Remove(item);
                    
                } break;
                case PoolTypeId.CircleEffect:
                {
                    _circleEffect.Despawn(item as CircleEffect);
                    _circleEffectSpawned.Remove(item);
                } break;
                case PoolTypeId.HealthPointView:
                {
                    _healthPointPool.Despawn(item as HealthPointView);
                    _healthPointSpawned.Remove(item);
                } break;
            }
        }

        private void AddItemToList<TItem>(TItem item, PoolTypeId poolTypeId) where TItem : MonoBehaviour
        {
            switch (poolTypeId)
            {
                case PoolTypeId.EntityView: _entitySpawned.Add(item); break;
                case PoolTypeId.CircleEffect: _circleEffectSpawned.Add(item); break;
                case PoolTypeId.HealthPointView: _healthPointSpawned.Add(item); break;
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
            
            foreach (MonoBehaviour healthPoint in _healthPointSpawned)
            {
                _healthPointPool.Despawn(healthPoint as HealthPointView);
            }
            
            _entitySpawned.Clear();
            _healthPointSpawned.Clear();
            _circleEffectSpawned.Clear();
        }
    }
}