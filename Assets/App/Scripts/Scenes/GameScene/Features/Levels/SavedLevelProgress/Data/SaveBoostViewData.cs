using System;
using App.Scripts.Scenes.GameScene.Features.Boosts.General;
using Newtonsoft.Json;

namespace App.Scripts.Scenes.GameScene.Features.Levels.SavedLevelProgress
{
    [Serializable]
    public class SaveBoostViewData
    {
        [JsonProperty("BoostType")]
        public BoostTypeId BoostTypeId;
        
        [JsonProperty("PositionX")]
        public float PositionX;
        [JsonProperty("PositionY")]
        public float PositionY;

        public SaveBoostViewData()
        {
        }

        [JsonConstructor]
        public SaveBoostViewData(BoostTypeId boostType, float positionX, float positionY)
        {
            BoostTypeId = boostType;
            PositionX = positionX;
            PositionY = positionY;
        }
        
        public SaveBoostViewData(BoostView boostView)
        {
            BoostTypeId = boostView.BoostTypeId;
            PositionX = boostView.transform.position.x;
            PositionY = boostView.transform.position.y;
        }
    }
}