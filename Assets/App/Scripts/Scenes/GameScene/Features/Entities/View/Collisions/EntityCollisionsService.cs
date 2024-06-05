using System.Collections.Generic;
using App.Scripts.External.ObjectPool;
using App.Scripts.General.Infrastructure;
using App.Scripts.Scenes.GameScene.Features.Effects;
using App.Scripts.Scenes.GameScene.Features.Effects.ObjectPool;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Entities.View.Collisions
{
    public sealed class EntityCollisionsService : IEntityCollisionService, IGeneralRestartable
    {
        private readonly List<IEntityView> _views;
        private readonly IKeyObjectPool<AbstractEffect> _keyPool;
        private readonly EffectCollisionProvider _effectCollisionProvider;

        public EntityCollisionsService(IKeyObjectPool<AbstractEffect> keyPool, EffectCollisionProvider effectCollisionProvider)
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
                    AbstractEffect effect = _keyPool.Spawn(effectName);

                    effect.Position = entity.Position;
                    effect.Scale = entity.Scale.x;

                    if (effect is CircleEffects circle)
                    {
                        circle.ScaleForSubParticles = effect.Scale / 10f;
                    }
                    
                    effect.PlayEffect();
                    effect.Disabled += OnEffectDisabled;
                }
            }
        }

        private void OnEffectDisabled(AbstractEffect effect)
        {
            _keyPool.Despawn(effect);
        }
    }
}