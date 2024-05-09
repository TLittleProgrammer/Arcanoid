using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Levels.Entities
{
    [Serializable]
    public class EntityCellData
    {
        [PreviewField(45)]
        public Sprite Sprite;
        public int HealthPoints;
    }
}