using System;
using System.Collections.Generic;
using App.Scripts.External.ObjectPool;
using UnityEditor;

namespace App.Scripts.Scenes.GameScene.Features.Effects.ObjectPool
{
    public sealed class EffectKeyObjectPool : IKeyObjectPool<AbstractEffect>
    {
        private Dictionary<string, MonoObjectPool<AbstractEffect>> _pools;

        public EffectKeyObjectPool(List<MonoObjectPool<AbstractEffect>> pools)
        {
            _pools = new();

            Initialize(pools);
        }

        private void Initialize(List<MonoObjectPool<AbstractEffect>> pools)
        {
            foreach (MonoObjectPool<AbstractEffect> pool in pools)
            {
                _pools.Add(pool.Key, pool);
            }
        }

        public AbstractEffect Spawn(string key)
        {
            if (_pools.ContainsKey(key))
            {
                return _pools[key].Spawn();
            }

            return null;
        }

        public void Despawn(AbstractEffect effect)
        {
            if (_pools.ContainsKey(effect.PoolKey))
            {
                _pools[effect.PoolKey].Despawn(effect);
            }
        }
    }
}