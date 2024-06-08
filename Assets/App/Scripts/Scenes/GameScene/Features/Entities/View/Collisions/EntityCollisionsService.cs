using System.Collections.Generic;
using App.Scripts.External.ObjectPool;
using App.Scripts.General.Infrastructure;
using App.Scripts.Scenes.GameScene.Features.Effects;
using App.Scripts.Scenes.GameScene.Features.Effects.Conditions;
using App.Scripts.Scenes.GameScene.Features.Effects.ObjectPool;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Entities.View.Collisions
{
    public sealed class EntityCollisionsService : IEntityCollisionService, IGeneralRestartable
    {
        private readonly List<IEntityView> _views;
        private readonly IKeyObjectPool<IEffect> _keyPool;
        private readonly EffectCollisionProvider _effectCollisionProvider;
        private readonly IConditionService _conditionService;

        public EntityCollisionsService(
            IKeyObjectPool<IEffect> keyPool,
            EffectCollisionProvider effectCollisionProvider,
            IConditionService conditionService)
        {
            _views = new();
            _keyPool = keyPool;
            _effectCollisionProvider = effectCollisionProvider;
            _conditionService = conditionService;
        }

        private Dictionary<int, List<ConditionToEffectMapping>> EffectMapping => _effectCollisionProvider.EntityIdToEffectNameMapping;

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
            if (EffectMapping.TryGetValue(entity.EntityId, out List<ConditionToEffectMapping> conditions))
            {
                foreach (ConditionToEffectMapping condition in conditions)
                {
                    if (_conditionService.Execute(condition.Condition.GetType(), entity, collider))
                    {
                        ActiveEffects(entity, collider, condition);
                    }
                }
            }
        }

        private void ActiveEffects(IEntityView entity, Collision2D collider, ConditionToEffectMapping condition)
        {
            foreach (string effectName in condition.EffectNames)
            {
                IEffect effect = _keyPool.Spawn(effectName);

                effect.PlayEffect(entity.GameObject.transform, collider.transform);

                effect.Disabled += disabledEffect => { OnEffectDisabled(effectName, disabledEffect); };
            }
        }

        private void OnEffectDisabled(string effectName, IEffect disabledEffect)
        {
            _keyPool.Despawn(effectName, disabledEffect);
        }
    }
}