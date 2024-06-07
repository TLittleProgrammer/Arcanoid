using System.Collections.Generic;
using App.Scripts.External.ObjectPool;
using App.Scripts.General.Infrastructure;
using App.Scripts.Scenes.GameScene.Features.Effects;
using App.Scripts.Scenes.GameScene.Features.Effects.ObjectPool;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Entities.View.Collisions
{
    public sealed class EntityCollisionsService : IEntityCollisionService, IGeneralRestartable
    {
        private readonly List<IEntityView> _views;
        private readonly IKeyObjectPool<IEffect> _keyPool;
        private readonly EffectCollisionProvider _effectCollisionProvider;

        public EntityCollisionsService(IKeyObjectPool<IEffect> keyPool, EffectCollisionProvider effectCollisionProvider)
        {
            _views = new();
            _keyPool = keyPool;
            _effectCollisionProvider = effectCollisionProvider;
        }

        private Dictionary<int, List<string>> EffectMapping => _effectCollisionProvider.EntityIdToEffectNameMapping;

        public void AddEntity(IEntityView entityView)
        {
            _views.Add(entityView);

            entityView.Colliderable += OnEntityCollidired;
        }

        public void Clear()
        {
            foreach (IEntityView view in _views)
            {
                view.Colliderable -= OnEntityCollidired;
            }
            
            _views.Clear();
        }

        public void Restart()
        {
            Clear();
        }

        private void OnEntityCollidired(IEntityView entity, Collision2D collider)
        {
            if (EffectMapping.TryGetValue(entity.EntityId, out List<string> effects))
            {
                foreach (string effectName in effects)
                {
                    IEffect effect = _keyPool.Spawn(effectName);

                    effect.PlayEffect(entity.GameObject.transform, collider.transform);

                    effect.Disabled += disabledEffect =>
                    {
                        OnEffectDisabled(effectName, disabledEffect);
                    };
                }
            }
        }

        private void OnEffectDisabled(string effectName, IEffect disabledEffect)
        {
            _keyPool.Despawn(effectName, disabledEffect);
        }
    }
}