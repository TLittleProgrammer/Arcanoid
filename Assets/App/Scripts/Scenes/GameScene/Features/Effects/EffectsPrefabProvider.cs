using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Effects
{
    [CreateAssetMenu(menuName = "Configs/Game/Prefabs/EffectsPrefabProvider", fileName = "EffectsPrefabProvider")]
    public class EffectsPrefabProvider : SerializedScriptableObject
    {
        public Dictionary<Type, EffectData> Provider;
    }
}