using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameScene.Levels.AssetManagement
{
    [CreateAssetMenu(menuName = "Configs/Level/EntitiesProvider", fileName = "EntitiesProvider")]
    public class EntitiesProvider : SerializedScriptableObject
    {
        public List<EntityProvider> EntitiesProviderList;
    }
}