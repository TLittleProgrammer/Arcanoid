using System;
using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.Features.Effects.Conditions;

namespace App.Scripts.Scenes.GameScene.Features.Entities.View.Collisions
{
    [Serializable]
    public class ConditionToEffectMapping
    {
        public ICondition Condition;
        public List<string> EffectNames;
    }
}