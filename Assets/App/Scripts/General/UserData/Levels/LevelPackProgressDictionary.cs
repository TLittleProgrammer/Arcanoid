using System.Collections.Generic;
using App.Scripts.External.UserData.SaveLoad;
using App.Scripts.General.UserData.Constants;

namespace App.Scripts.General.UserData.Levels
{
    public sealed class LevelPackProgressDictionary : Dictionary<int, LevelPackProgressProgressData>, ISavable, ILevelPackProgress
    {
        public string FileName => SavableConstants.LevelProgressFileName;
        public int GetPassedLevelCount(int packIndex)
        {
            if (TryGetValue(packIndex, out LevelPackProgressProgressData levelPackProgressProgressData))
            {
                return levelPackProgressProgressData.PassedLevels;
            }

            return 0;
        }
    }
}