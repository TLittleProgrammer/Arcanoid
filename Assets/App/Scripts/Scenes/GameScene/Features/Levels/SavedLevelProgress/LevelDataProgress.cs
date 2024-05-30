using System;
using System.Collections.Generic;
using App.Scripts.External.Grid;
using App.Scripts.External.UserData.SaveLoad;
using App.Scripts.General.Levels;
using App.Scripts.General.UserData.Constants;
using App.Scripts.Scenes.GameScene.Features.Entities;
using App.Scripts.Scenes.GameScene.Features.Grid;
using App.Scripts.Scenes.GameScene.Features.Levels.General;
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

        [JsonProperty("GridSizeX")]
        public int GridSizeX;
        [JsonProperty("GridSizeY")]
        public int GridSizeY;

        [JsonProperty("LevelData")]
        public LevelData LevelData;
        
        [JsonProperty("EntityDatas")]
        public List<EntitySaveData> EntityDatas;
        [JsonProperty("EntityGridItemDatas")]
        public List<SaveGridItemData> EntityGridItemsData;

        [JsonProperty("ActiveBoostData")]
        public List<SaveActiveBoostData> ActiveBoostDatas;
        
        [JsonProperty("ViewBoostData")]
        public List<SaveBoostViewData> ViewBoostDatas;
        
        public string FileName => SavableConstants.CurrentLevelProgressFileName;
    }
}