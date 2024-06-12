using System;
using App.Scripts.External.UserData.SaveLoad;
using App.Scripts.General.UserData.Constants;
using Newtonsoft.Json;

namespace App.Scripts.General.UserData.Global
{
    [Serializable]
    public class GlobalData : ISavable
    {
        [JsonProperty("IsFirstEnter")]
        public bool IsFirstEnter = true;
        [JsonProperty("LastTimestampEnter")]
        public long LastTimestampEnter;
    }
}