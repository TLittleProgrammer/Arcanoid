using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Levels.SavedLevelProgress
{
    [Serializable]
    public class LevelPackData
    {
        [JsonProperty("LocaleKey")]
        public string LocaleKey;
        [JsonIgnore]
        public string GalacticIcon;
        [JsonIgnore]
        public string GalacticBackground;
        [JsonProperty("EnergyPrice")]
        public int EnergyPrice = 3;
        [JsonProperty("EnergyReward")]
        public int EnergyAddForWin = 5;
        [JsonProperty("Levels")]
        public List<string> Levels;
    }
}