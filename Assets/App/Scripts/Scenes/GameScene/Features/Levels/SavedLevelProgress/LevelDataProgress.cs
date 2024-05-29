

using System;
using System.Collections.Generic;
using App.Scripts.External.Grid;
using App.Scripts.External.UserData.SaveLoad;
using App.Scripts.General.UserData.Constants;
using App.Scripts.Scenes.GameScene.Features.Entities;
using App.Scripts.Scenes.GameScene.Features.Grid;
using Newtonsoft.Json;

namespace App.Scripts.Scenes.GameScene.Features.Levels.SavedLevelProgress
{
    [Serializable]
    public class LevelDataProgress : ISavable
    {
        [JsonProperty("MaxHealth")]
        public int AllHealthes;
        [JsonProperty("ActualHealth")]
        public int CurrentHealth;
        
        [JsonProperty("EntityDatas")]
        public List<EntitySaveData> EntityDatas;
        [JsonProperty("EntityGridItemDatas")]
        public Grid<GridItemData> EntityGridItemsData;
        
        public string FileName => SavableConstants.CurrentLevelProgressFileName;
    }
}