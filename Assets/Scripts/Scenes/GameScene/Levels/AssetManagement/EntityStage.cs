using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameScene.Levels.AssetManagement
{
    [CreateAssetMenu(menuName = "Configs/Level/EntityStage", fileName = "EntityStage")]
    public sealed class EntityStage : ScriptableObject
    {
        [PreviewField(50),
        HorizontalGroup("MainParameters")]
        public Sprite Sprite;

        [HorizontalGroup("MainParameters")]
        public int HealthCounter;

        [PreviewField(50)]
        public List<Sprite> AvailableSpritesOnMainSprite;
    }
}