using System;
using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.Entities;
using App.Scripts.Scenes.GameScene.TopSprites;

namespace App.Scripts.Scenes.GameScene.Levels.Data
{
    [Serializable]
    public class GridItemData
    {
        public BoostTypeId BoostTypeId;
        public int Damage;
        public int CurrentHealth;
        public List<OnTopSprites> Sprites = new();
    }
}