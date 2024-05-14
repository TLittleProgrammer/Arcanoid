using System;
using Unity.Mathematics;
using Unity.Plastic.Newtonsoft.Json;

namespace App.Scripts.Scenes.GameScene.Levels
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
        [JsonProperty("Grid")]
        public int[,] Grid;
    }
}