using System.Collections.Generic;
using App.Scripts.External.ObjectPool;

namespace App.Scripts.Scenes.GameScene.Features.Effects.ObjectPool
{
    public sealed class EffectKeyObjectPool : IKeyObjectPool<AbstractEffect>
    {
        private Dictionary<string, MonoObjectPool<AbstractEffect>> _pools;

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