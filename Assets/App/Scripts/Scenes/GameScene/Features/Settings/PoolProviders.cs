using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.Pools;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Settings
{
    [CreateAssetMenu(menuName = "Configs/Game Settings/Pool Provider", fileName = "PoolProvider")]
    public class PoolProviders : SerializedScriptableObject
    {
        public Dictionary<PoolTypeId, PoolSettings> Pools;
    }
}