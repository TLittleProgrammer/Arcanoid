using System.Collections.Generic;
using UnityEngine;

namespace GameScene.Levels.AssetManagement
{
    [CreateAssetMenu(menuName = "Configs/Level/EntitiesProvider", fileName = "EntitiesProvider")]
    public class EntitiesProvider : ScriptableObject
    {
        public List<EntityProvider> EntitiesProviderList;
    }
}