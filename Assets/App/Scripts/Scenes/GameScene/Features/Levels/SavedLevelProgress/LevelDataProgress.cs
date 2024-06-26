﻿using System;
using System.Collections.Generic;
using App.Scripts.External.UserData.SaveLoad;
using App.Scripts.Scenes.GameScene.Features.Entities;
using App.Scripts.Scenes.GameScene.Features.Levels.General;
using App.Scripts.Scenes.GameScene.Features.Levels.SavedLevelProgress.Data;
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

        [JsonProperty("PlatformData")]
        public PlatformSaveData PlatformData;

        [JsonProperty("BallsData")] 
        public BallsSaveData BallsData;
        
        [JsonProperty("LevelData")]
        public LevelData LevelData;
        
        [JsonProperty("ProgressedLevelData")]
        public ProgressedLevelData ProgressedLevelData;
        
        [JsonProperty("EntityDatas")]
        public List<EntitySaveData> EntityDatas;
        [JsonProperty("EntityGridItemDatas")]
        public List<SaveGridItemData> EntityGridItemsData;

        [JsonProperty("ActiveBoostData")]
        public List<SaveActiveBoostData> ActiveBoostDatas;
        
        [JsonProperty("ViewBoostData")]
        public List<SaveBoostViewData> ViewBoostDatas;

        [JsonProperty("LevelPack")]
        public LevelTransferPackData LevelTransferPackData;
    }
}