using System.Collections.Generic;
using App.Scripts.General.UserData.Constants;
using App.Scripts.General.UserData.SaveLoad;

namespace App.Scripts.General.UserData.Data
{
    public sealed class LevelPackProgressDictionary : Dictionary<int, LevelPackProgressProgressData>, ISavable
    {
        public string FileName => SavableConstants.LevelProgressFileName;
    }
}