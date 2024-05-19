using System;
using App.Scripts.External.UserData.SaveLoad;
using App.Scripts.General.UserData.Constants;
using Newtonsoft.Json;

namespace App.Scripts.General.UserData.Persistent
{
    [Serializable]
    public class PersistentData : ISavable
    {
        [JsonProperty("LastVisitTimeUTC")]
        public long LastVisit;
        [JsonProperty("IsFirstEnter")]
        public bool IsFirstEnter = true;
        
        [JsonIgnore]
        public string FileName => SavableConstants.PersistentFileName;
    }
}