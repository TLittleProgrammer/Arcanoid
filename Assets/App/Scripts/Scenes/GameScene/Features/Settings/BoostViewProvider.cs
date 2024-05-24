using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.Features.Entities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Settings
{
    [CreateAssetMenu(menuName = "Configs/Game/BoostViewProvider", fileName = "BoostViewProvider")]
    public class BoostViewProvider : SerializedScriptableObject
    {
        public Dictionary<BoostTypeId, Sprite> Sprites;
        public Dictionary<BoostTypeId, Sprite> Icons;
    }
}