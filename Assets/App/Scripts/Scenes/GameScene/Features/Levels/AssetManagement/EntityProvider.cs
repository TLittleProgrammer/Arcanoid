using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Levels.AssetManagement
{
    [CreateAssetMenu(menuName = "Configs/Level/EntityProvider", fileName = "EntityProvider")]
    public class EntityProvider : SerializedScriptableObject
    {
        public Dictionary<string, EntityStage> EntityStages;
    }
}