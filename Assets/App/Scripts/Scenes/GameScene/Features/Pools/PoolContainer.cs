using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.Effects;
using App.Scripts.Scenes.GameScene.Entities;
using App.Scripts.Scenes.GameScene.Healthes.View;
using App.Scripts.Scenes.GameScene.TopSprites;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Pools
{
    public sealed class PoolContainer : IPoolContainer
    {
        private readonly EntityView.Pool _entityViewPool;
        private readonly IEffect<CircleEffect>.Pool _circleEffectPool;
        private readonly HealthPointView.Pool _healthPointPool;
        private readonly OnTopSprites.Pool _onTopSpritesPool;

        private List<MonoBehaviour> _entitySpawned = new();
        private List<MonoBehaviour> _circleEffectSpawned = new();
        private List<MonoBehaviour> _healthPointSpawned = new();
        private List<MonoBehaviour> _onTopSpritesSpawned = new();

        private Dictionary<PoolTypeId, List<MonoBehaviour>> _spawnedItemDictionaryHelper;

        public PoolContainer(
            EntityView.Pool entityViewPool,
            IEffect<CircleEffect>.Pool circleEffectPool,
            HealthPointView.Pool healthPointPool,
            OnTopSprites.Pool onTopSpritesPool)
        {
            _circleEffectPool = circleEffectPool;
            _healthPointPool = healthPointPool;
            _onTopSpritesPool = onTopSpritesPool;
            _entityViewPool = entityViewPool;

            _spawnedItemDictionaryHelper = new()
            {
                [PoolTypeId.EntityView]      = _entitySpawned,
                [PoolTypeId.CircleEffect]    = _circleEffectSpawned,
                [PoolTypeId.HealthPointView] = _healthPointSpawned,
                [PoolTypeId.OnTopSprite]     = _onTopSpritesSpawned,
            };
        }

        public TItem GetItem<TItem>(PoolTypeId poolTypeId) where TItem : MonoBehaviour
        {
            TItem item = poolTypeId switch
            {
                PoolTypeId.EntityView      => _entityViewPool.Spawn().GetComponent<TItem>(),
                PoolTypeId.CircleEffect    => _circleEffectPool.Spawn().GetComponent<TItem>(),
                PoolTypeId.HealthPointView => _healthPointPool.Spawn().GetComponent<TItem>(),
                PoolTypeId.OnTopSprite     => _onTopSpritesPool.Spawn().GetComponent<TItem>(),
                
                _ => null
            };

            _spawnedItemDictionaryHelper[poolTypeId].Add(item);

            return item;
        }

        public void RemoveItem<TItem>(PoolTypeId poolTypeId, TItem item) where TItem : MonoBehaviour
        {
            switch (poolTypeId)
            {
                case PoolTypeId.EntityView: RemoveItemFromPool(item, _entityViewPool, _entitySpawned); break;
                case PoolTypeId.CircleEffect: RemoveItemFromPool(item, _circleEffectPool, _circleEffectSpawned); break;
                case PoolTypeId.HealthPointView: RemoveItemFromPool(item, _healthPointPool, _healthPointSpawned); break;
                case PoolTypeId.OnTopSprite: RemoveItemFromPool(item, _onTopSpritesPool, _onTopSpritesSpawned); break;
            }
        }

        private void RemoveItemFromPool<TItem, TMono>(TItem item, MemoryPoolBase<TMono> poolBase,
            List<MonoBehaviour> spawnedList)
            where TMono : MonoBehaviour
            where TItem : MonoBehaviour
        {
            if (spawnedList.Contains(item))
            {

                poolBase.Despawn(item as TMono);
                spawnedList.Remove(item);
            }
        }

        public void Restart()
        {
            Clear<EntityView>(_entitySpawned, _entityViewPool);
            Clear<CircleEffect>(_circleEffectSpawned, _circleEffectPool);
            Clear<HealthPointView>(_healthPointSpawned, _healthPointPool);
            Clear<OnTopSprites>(_onTopSpritesSpawned, _onTopSpritesPool);
        }

        private void Clear<TView>(List<MonoBehaviour> spawned, IMemoryPool memoryPool) where TView : MonoBehaviour
        {
            foreach (MonoBehaviour entity in spawned)
            {
                memoryPool.Despawn(entity as TView);
            }
            
            spawned.Clear();
        }
    }
}