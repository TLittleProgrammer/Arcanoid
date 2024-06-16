using System;
using App.Scripts.Scenes.GameScene.Features.Boosts.General;
using Newtonsoft.Json;

namespace App.Scripts.Scenes.GameScene.Features.Levels.SavedLevelProgress.Data
{
    [Serializable]
    public class SaveBoostViewData
    {
        [JsonProperty("BoostType")]
        public string BoostTypeId;
        
        [JsonProperty("PositionX")]
        public float PositionX;
        [JsonProperty("PositionY")]
        public float PositionY;
        [JsonProperty("ScaleX")]
        public float ScaleX;
        [JsonProperty("ScaleY")]
        public float ScaleY;


        public SaveBoostViewData()
        {
        }

        [JsonConstructor]
        public SaveBoostViewData(string boostType, float positionX, float positionY)
        {
            BoostTypeId = boostType;
            PositionX = positionX;
            PositionY = positionY;
        }
        
        public SaveBoostViewData(BoostView boostView)
        {
            BoostTypeId = boostView.BoostTypeId;
            
            var transform = boostView.transform;
            
            PositionX = transform.position.x;
            PositionY = transform.position.y;

            ScaleX = transform.localScale.x;
            ScaleY = transform.localScale.y;
        }
    }
}