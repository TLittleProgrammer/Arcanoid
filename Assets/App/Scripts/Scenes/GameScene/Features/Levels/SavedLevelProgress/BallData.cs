using System;
using App.Scripts.External.CustomData;
using Newtonsoft.Json;

namespace App.Scripts.Scenes.GameScene.Features.Levels.SavedLevelProgress
{
    [Serializable]
    public class BallData
    {
        [JsonProperty("Position")]
        public PositionData Position;
        [JsonProperty("Velocity")]
        public Float2 Velocity;
        [JsonProperty("IsFreeFlight")]
        public bool IsFreeFlight;
    }
}