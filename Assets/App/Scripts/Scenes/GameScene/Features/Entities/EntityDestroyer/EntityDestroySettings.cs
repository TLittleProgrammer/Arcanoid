using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.Features.Entities.EntityDestroyer.DestroyServices;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Entities.EntityDestroyer
{
    [CreateAssetMenu(menuName = "Configs/EntityDestroySettings", fileName = "EntityDestroySettings")]
    public class EntityDestroySettings : SerializedScriptableObject
    {
        public List<DestroySettingsServiceData> DestroyServiceDatas;
        public List<string> ActiveBoosts;
    }

    public class DestroySettingsServiceData
    {
        public IBlockDestroyService BlockDestroyService;
        public List<string> DestroyingIds;
    }
}