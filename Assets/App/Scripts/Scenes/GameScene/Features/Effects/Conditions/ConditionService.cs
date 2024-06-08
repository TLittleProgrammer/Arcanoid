using System;
using System.Collections.Generic;
using System.Linq;
using App.Scripts.Scenes.GameScene.Features.Entities.View;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Effects.Conditions
{
    public sealed class ConditionService : IConditionService
    {
        private readonly Dictionary<Type,ICondition> _conditions;

        public ConditionService(List<ICondition> conditions)
        {
            _conditions = conditions.ToDictionary(x => x.GetType(), x => x);
        }

        public bool Execute(Type conditionType, IEntityView entityView, Collider2D collision2D)
        {
            if (_conditions.TryGetValue(conditionType, out ICondition condition))
            {
                return condition.Execute(entityView, collision2D);
            }

            return false;
        }
    }
}