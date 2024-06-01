using System;
using Newtonsoft.Json;

namespace App.Scripts.External.CustomData
{
    [Serializable]
    public class PositionData
    {
        [JsonProperty("X")]
        public float X;
        [JsonProperty("Y")]
        public float Y;
        [JsonProperty("Z")]
        public float Z;
    }
}