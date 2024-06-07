using System.Collections.Generic;
using App.Scripts.External.ObjectPool;

namespace App.Scripts.Scenes.GameScene.Features.Effects.ObjectPool
{
    public sealed class KeyObjectPool<TType> : IKeyObjectPool<TType>
    {
        private Dictionary<string, IObjectPool<TType>> _pools;
        
        public KeyObjectPool(List<IKeyableObjectPool<TType>> pools)
        {
            _pools = new();

            Initialize(pools);
        }

        private void Initialize(List<IKeyableObjectPool<TType>> pools)
        {
            foreach (IKeyableObjectPool<TType> pool in pools)
            {
                _pools.Add(pool.Key, pool);
            }
        }

        public TType Spawn(string key)
        {
            return _pools[key].Spawn();
        }

        public void Despawn(string key, object item)
        {
            if (_pools[key] is IObjectPool objectPool)
            {
                objectPool.Despawn(item);
            }
        }

        public void Clear()
        {
            _pools.Clear();
        }
    }
}