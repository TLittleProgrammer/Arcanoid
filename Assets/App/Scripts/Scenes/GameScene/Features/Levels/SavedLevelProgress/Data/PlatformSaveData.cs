﻿using System;
using App.Scripts.External.CustomData;
using Newtonsoft.Json;

namespace App.Scripts.Scenes.GameScene.Features.Levels.SavedLevelProgress.Data
{
    [Serializable]
    public class PlatformSaveData
    {
        [JsonProperty("Position")]
        public PositionData Position;
    }
}