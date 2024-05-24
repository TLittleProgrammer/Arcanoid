﻿using System;
using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.Features.Entities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Levels.AssetManagement
{
    [CreateAssetMenu(menuName = "Configs/Level/EntityStage", fileName = "EntityStage")]
    public sealed class EntityStage : ScriptableObject
    {
        public BoostTypeId BoostTypeId;
        [ShowIf("BoostTypeId", BoostTypeId.Bomb)]
        public int Damage = 0;
        

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