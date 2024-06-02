using System;
using App.Scripts.Scenes.GameScene.Features.Boosts.General;
using App.Scripts.Scenes.GameScene.Features.Grid;
using Newtonsoft.Json;

namespace App.Scripts.Scenes.GameScene.Features.Levels.SavedLevelProgress.Data
{
    [Serializable]
    public class SaveGridItemData
    {
        [JsonProperty("BoostTypeId")]
        public BoostTypeId BoostTypeId;
        [JsonProperty("CurrentHealth")]
        public int CurrentHealth;

        [JsonProperty("PositionX")]
        public int GridPositionX;
        [JsonProperty("PositionY")]
        public int GridPositionY;

        public SaveGridItemData()
        {
        }

        [JsonConstructor]
        public SaveGridItemData(BoostTypeId boostTypeId, int currentHealth, int gridPositionX, int gridPositionY)
        {
            BoostTypeId = boostTypeId;
            CurrentHealth = currentHealth;
            GridPositionX = gridPositionX;
            GridPositionY = gridPositionY;
        } 
        
        public SaveGridItemData(GridItemData gridItemData, int x, int y)
        {
            BoostTypeId = gridItemData.BoostTypeId;
            CurrentHealth = gridItemData.CurrentHealth;
            GridPositionX = x;
            GridPositionY = y;
        }
    }
}