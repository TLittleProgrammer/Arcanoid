using System;
using Newtonsoft.Json;

namespace App.Scripts.Scenes.GameScene.Features.Levels.SavedLevelProgress
{
    [Serializable]
    public sealed class ProgressedLevelData
    {
        [JsonProperty("DestroyedBlocksCounter")]
        public int DestroyedBlocks;
        [JsonProperty("AllBlocksCounter")]
        public int AllBlocksCounter;
        [JsonProperty("Progress")]
        public float Progress;
        [JsonProperty("Step")]
        public float Step;
    }
}