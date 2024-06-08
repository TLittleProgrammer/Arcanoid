using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Entities.View.Collisions
{
    [CreateAssetMenu(menuName = "Configs/Game/Effects/EffectsProvider", fileName = "CollisionsEffectsProvider")]
    public class EffectCollisionProvider : SerializedScriptableObject
    {
        public Dictionary<int, List<ConditionToEffectMapping>> EntityIdToEffectNameMapping;
    }
}