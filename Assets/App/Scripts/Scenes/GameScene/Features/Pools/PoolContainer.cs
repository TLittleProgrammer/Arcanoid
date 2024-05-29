using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.Features.Boosts;
using App.Scripts.Scenes.GameScene.Features.Boosts.General;
using App.Scripts.Scenes.GameScene.Features.Effects;
using App.Scripts.Scenes.GameScene.Features.Entities;
using App.Scripts.Scenes.GameScene.Features.Entities.TopSprites;
using App.Scripts.Scenes.GameScene.Features.Entities.View;
using App.Scripts.Scenes.GameScene.Features.Healthes.View;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Pools
{
    public sealed class PoolContainer : IPoolContainer
    {
        private readonly EntityView.Pool _entityViewPool;
        private readonly IEffect<CircleEffect>.Pool _circleEffectPool;
        private readonly HealthPointView.Pool _healthPointPool;
        private readonly OnTopSprites.Pool _onTopSpritesPool;
        private readonly BoostView.Pool _boostsViewPool;

        private List<MonoBehaviour> _entitySpawned = new();
        private List<MonoBehaviour> _circleEffectSpawned = new();
        private List<MonoBehaviour> _healthPointSpawned = new();
        private List<MonoBehaviour> _onTopSpritesSpawned = new();
        private List<MonoBehaviour> _boostsViewSpawned = new();

        private Dictionary<PoolTypeId, List<MonoBehaviour>> _spawnedItemDictionaryHelper;

        public PoolContainer(
            EntityView.Pool entityViewPool,
            IEffect<CircleEffect>.Pool circleEffectPool,
            HealthPointView.Pool healthPointPool,
            OnTopSprites.Pool onTopSpritesPool,
            BoostView.Pool boostsViewPool)
        {
            _circleEffectPool = circleEffectPool;
            _healthPointPool = healthPointPool;
            _onTopSpritesPool = onTopSpritesPool;
            _boostsViewPool = boostsViewPool;
            _entityViewPool = entityViewPool;

            _spawnedItemDictionaryHelper = new()
            {
                [PoolTypeId.EntityView]      = _entitySpawned,
                [PoolTypeId.CircleEffect]    = _circleEffectSpawned,
                [PoolTypeId.HealthPointView] = _healthPointSpawned,
                [PoolTypeId.OnTopSprite]     = _onTopSpritesSpawned,
                [PoolTypeId.Boosts]          = _boostsViewSpawned,
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
                PoolTypeId.Boosts          => _boostsViewPool.Spawn().GetComponent<TItem>(),
                
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
                case PoolTypeId.Boosts: RemoveItemFromPool(item, _boostsViewPool, _boostsViewSpawned); break;
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
            Clear<BoostView>(_boostsViewSpawned, _boostsViewPool);
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