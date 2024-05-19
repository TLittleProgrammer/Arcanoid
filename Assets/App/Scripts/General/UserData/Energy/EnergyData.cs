using App.Scripts.External.UserData.SaveLoad;
using App.Scripts.General.Constants;
using App.Scripts.General.UserData.Constants;
using Newtonsoft.Json;

namespace App.Scripts.General.UserData.Energy
{
    public class EnergyData : ISavable
    {
        [JsonProperty("EnergyCounter")]
        public int EnergyCounter = UserDataConstants.DefaultEnergyCount;

        [JsonIgnore] public string FileName => SavableConstants.EnergyFileName;
    }
}