using System;
using Newtonsoft.Json;

namespace App.Scripts.Scenes.GameScene.Features.Levels.SavedLevelProgress.Data
{
    [Serializable]
    public class LevelTransferPackData
    {
        [JsonProperty("NeedLoadLevel")]
        public bool NeedLoadLevel;
        [JsonProperty("NeedContinue")]
        public bool NeedContinue;
        [JsonProperty("PackIndex")]
        public int PackIndex;
        [JsonProperty("LevelIndex")]
        public int LevelIndex;
        [JsonProperty("LevelPack")]
        public LevelPackData LevelPack;
    }
}