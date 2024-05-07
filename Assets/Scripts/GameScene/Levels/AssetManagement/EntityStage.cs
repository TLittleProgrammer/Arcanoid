using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameScene.Levels.AssetManagement
{
    [Serializable]
    public sealed class EntityStage
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