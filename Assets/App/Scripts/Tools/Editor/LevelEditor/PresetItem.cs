using System;
using Newtonsoft.Json;

namespace Editor.LevelEditor
{
    [Serializable]
    public class PresetItem
    {
        [JsonProperty("blockKey")]
        public string BlockKey;
    }
}