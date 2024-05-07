using System.Collections.Generic;
using GameScene.Levels.Entities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameScene.Levels.AssetManagement
{
    [CreateAssetMenu(menuName = "Configs/Level/EntityProvider", fileName = "EntityProvider")]
    public class EntityProvider : SerializedScriptableObject
    {
        public EntityTypeId EntityTypeId;
        
        public List<EntityStage> EntityStages;
    }
}