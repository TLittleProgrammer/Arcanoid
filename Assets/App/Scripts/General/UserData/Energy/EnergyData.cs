using System;
using App.Scripts.External.UserData.SaveLoad;
using App.Scripts.General.UserData.Constants;
using Newtonsoft.Json;

namespace App.Scripts.General.UserData.Energy
{
    [Serializable]
    public class EnergyData : ISavable
    {
        [JsonProperty("EnergyValue")]
        public int Value;

        [JsonIgnore]
        public string FileName => SavableConstants.EnergyFileName;
    }
}