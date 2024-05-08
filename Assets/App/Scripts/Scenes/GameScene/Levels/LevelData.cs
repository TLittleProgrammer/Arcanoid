using System;
using Unity.Mathematics;
using Unity.Plastic.Newtonsoft.Json;

namespace App.Scripts.Scenes.GameScene.Levels
{
    [Serializable]
    public class LevelData
    {
        [JsonProperty("GridSize")]
        public int2 GridSize;
        [JsonProperty("Grid")]
        public int[,] Grid;
    }
}