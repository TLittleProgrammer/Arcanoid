using System;
using Unity.Plastic.Newtonsoft.Json;

namespace App.Scripts.External.CustomData
{
    [Serializable]
    public class Float2
    {
        [JsonProperty("X")]
        public float X;
        [JsonProperty("Y")]
        public float Y;

        public Float2(float x, float y)
        {
            X = x;
            Y = y;
        }
    }
}