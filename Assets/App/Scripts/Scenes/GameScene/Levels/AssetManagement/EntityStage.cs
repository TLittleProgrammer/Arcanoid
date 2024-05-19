using System;
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

        [PreviewField(50), HorizontalGroup("MainParameters")]
        public Sprite Sprite;

        [HorizontalGroup("MainParameters")]
        public int MaxHealthCounter;

        public bool ICanGetDamage;
        public List<HealthSpriteData> AddSpritesOnMainByHp;
    }

    [Serializable]
    public class HealthSpriteData
    {
        public int Healthes;
        [PreviewField(50)]
        public List<Sprite> Sprites;
    }
}