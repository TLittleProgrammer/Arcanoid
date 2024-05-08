using System;
using Newtonsoft.Json;

namespace App.Scripts.Tools.Editor.LevelEditor
{
    [Serializable]
    public class PresetItem
    {
        [JsonProperty("blockKey")]
        public string BlockKey;
    }
}