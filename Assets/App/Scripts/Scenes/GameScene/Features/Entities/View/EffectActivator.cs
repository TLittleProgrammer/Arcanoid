using System.Collections.Generic;
using App.Scripts.External.ObjectPool;
using App.Scripts.Scenes.GameScene.Features.Effects;
using App.Scripts.Scenes.GameScene.Features.Effects.Conditions;
using App.Scripts.Scenes.GameScene.Features.Entities.View.Collisions;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Entities.View
{
    public sealed class EffectActivator : IEffectActivator
    {
        private readonly IKeyObjectPool<IEffect> _keyPool;
        private readonly EffectCollisionProvider _effectCollisionProvider;
        private readonly IConditionService _conditionService;

        public EffectActivator(
            IKeyObjectPool<IEffect> keyPool,
            EffectCollisionProvider effectCollisionProvider,
            IConditionService conditionService)
        {
            _keyPool = keyPool;
            _effectCollisionProvider = effectCollisionProvider;
            _conditionService = conditionService;
        }
        
        public void ActivateEffect(IEntityView entityView, Collider2D collider)
        {
            if (_effectCollisionProvider.EntityIdToEffectNameMapping.TryGetValue(entityView.EntityId, out List<ConditionToEffectMapping> conditions))
            {
                foreach (ConditionToEffectMapping condition in conditions)
                {
                    if (_conditionService.Execute(condition.Condition.GetType(), entityView, collider))
                    {
                        ActiveEffects(entityView, collider, condition);
                    }
                }
            }
        }
        
        private void ActiveEffects(IEntityView entity, Collider2D collider, ConditionToEffectMapping condition)
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