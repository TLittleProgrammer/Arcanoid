using System;
using Newtonsoft.Json;

namespace App.Scripts.Scenes.GameScene.Features.Entities
{
    [Serializable]
    public class EntitySaveData
    {
        [JsonProperty("EntityId")]
        public int EntityId;
        [JsonProperty("GridPositionX")]
        public int GridPositionX;
        [JsonProperty("GridPositionY")]
        public int GridPositionY;
    }
}