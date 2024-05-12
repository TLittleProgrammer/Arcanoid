using System.Collections.Generic;
using App.Scripts.External.UserData.SaveLoad;
using App.Scripts.General.UserData.Constants;

namespace App.Scripts.General.UserData.Data
{
    public sealed class LevelPackProgressDictionary : Dictionary<int, LevelPackProgressProgressData>, ISavable
    {
        public string FileName => SavableConstants.LevelProgressFileName;
    }
}