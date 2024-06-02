using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace App.Scripts.Scenes.GameScene.Features.Levels.SavedLevelProgress.Data
{
    [Serializable]
    public class LevelPackData
    {
        [JsonProperty("LocaleKey")]
        public string LocaleKey;
        [JsonProperty("GalacticIconKey")]
        public string GalacticIcon;
        [JsonProperty("GalacticBackgroundKey")]
        public string GalacticBackground;
        [JsonProperty("EnergyPrice")]
        public int EnergyPrice = 3;
        [JsonProperty("EnergyReward")]
        public int EnergyAddForWin = 5;
        [JsonProperty("Levels")]
        public List<string> Levels;
    }
}