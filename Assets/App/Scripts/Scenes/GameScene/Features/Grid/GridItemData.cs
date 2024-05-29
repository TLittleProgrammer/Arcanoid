using System;
using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.Features.Boosts.General;
using App.Scripts.Scenes.GameScene.Features.Entities.TopSprites;
using Newtonsoft.Json;

namespace App.Scripts.Scenes.GameScene.Features.Grid
{
    [Serializable]
    public class GridItemData
    {
        [JsonProperty("BoostTypeId")]
        public BoostTypeId BoostTypeId;
        [JsonProperty("Damage")]
        public int Damage;
        [JsonProperty("CanGetDamge")]
        public bool CanGetDamage;
        [JsonProperty("CurrentHealth")]
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