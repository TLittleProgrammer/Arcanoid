using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Settings
{
    [CreateAssetMenu(menuName = "Configs/Game Settings/Destroy Effects Mapping", fileName = "DestroyEntityEffectMapping")]
    public class DestroyEntityEffectMapping : SerializedScriptableObject
    {
        public Dictionary<string, List<string>> DestroyEffectsMapping;
    }
}