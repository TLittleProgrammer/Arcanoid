using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace App.Scripts.Scenes.GameScene.Features.Levels.SavedLevelProgress
{
    [Serializable]
    public class BallsSaveData
    {
        [JsonProperty("SpeedMultplier")]
        public float SpeedMultiplier;
        [JsonProperty("BallDatas")]
        public List<BallData> BallDatas;
    }
}