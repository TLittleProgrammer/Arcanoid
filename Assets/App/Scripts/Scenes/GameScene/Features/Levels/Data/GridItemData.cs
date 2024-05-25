using System;
using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.Features.Entities;
using App.Scripts.Scenes.GameScene.Features.TopSprites;

namespace App.Scripts.Scenes.GameScene.Features.Levels.Data
{
    [Serializable]
    public class GridItemData
    {
        public BoostTypeId BoostTypeId;
        public int Damage;
        public bool CanGetDamage;
        public int CurrentHealth;
        public List<OnTopSprites> Sprites = new();

        public GridItemData()
        {
            
        }
        
        public GridItemData(int health)
        {
            CurrentHealth = health;
        }
    }
}