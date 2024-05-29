using System;
using Newtonsoft.Json;
using Unity.Mathematics;

namespace App.Scripts.Scenes.GameScene.Features.Levels
{
    [Serializable]
    public class LevelData
    {
        [JsonProperty("HealthCount")]
        public int HealthCount;
        [JsonProperty("GridSize")]
        public int2 GridSize;
        [JsonProperty("OffsetBetweenCells")]
        public int2 OffsetBetweenCells;
        [JsonProperty("HorizontalOffset")]
        public int HorizontalOffset;
        [JsonProperty("TopOffset")]
        public int TopOffset;
        [JsonProperty("NeedBird")]
        public bool NeedBird;
        [JsonProperty("Grid")]
        public int[,] Grid;
    }
}