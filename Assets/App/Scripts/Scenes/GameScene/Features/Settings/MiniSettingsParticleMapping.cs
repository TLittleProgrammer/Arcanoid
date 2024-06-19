using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Settings
{
    [CreateAssetMenu(menuName = "Configs/Color", fileName = "MiniSettingsParticleMapping")]
    public class MiniSettingsParticleMapping : SerializedScriptableObject
    {
        public Dictionary<int, Color> Mapping;
    }
}