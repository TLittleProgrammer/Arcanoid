using System;
using App.Scripts.Scenes.GameScene.Features.Boosts.General;
using Newtonsoft.Json;

namespace App.Scripts.Scenes.GameScene.Features.Levels.SavedLevelProgress.Data
{
    [Serializable]
    public class SaveActiveBoostData
    {
        [JsonProperty("Type")]
        public BoostTypeId BoostTypeId;
        [JsonProperty("Duration")]
        public float Duration;

        public SaveActiveBoostData()
        {
        }

        [JsonConstructor]
        public SaveActiveBoostData(BoostTypeId type, float duration)
        {
            BoostTypeId = type;
            Duration = duration;
        }
        
        public SaveActiveBoostData(BoostData boostData)
        {
            BoostTypeId = boostData.BoostTypeId;
            Duration = boostData.Duration;
        }
    }
}