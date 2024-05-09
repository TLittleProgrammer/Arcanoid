using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.Entities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Levels.AssetManagement
{
    [CreateAssetMenu(menuName = "Configs/Level/EntityStage", fileName = "EntityStage")]
    public sealed class EntityStage : ScriptableObject
    {
        public EntityTypeId EntityTypeId;

        [PreviewField(50),
         HorizontalGroup("MainParameters")]
        public Sprite Sprite;

        [HorizontalGroup("MainParameters")]
        public int HealthCounter;

        public bool ICanGetDamage;

        [ShowIf("ICanGetDamage")]
        public EntityStage NextStage;

        [PreviewField(50)]
        public List<Sprite> AvailableSpritesOnMainSprite;
    }
}