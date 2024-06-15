using System;
using App.Scripts.External.UserData.SaveLoad;
using Newtonsoft.Json;

namespace App.Scripts.General.UserData.Energy
{
    [Serializable]
    public class EnergyData : ISavable
    {
        [JsonProperty("EnergyValue")]
        public int Value;
    }
}