using App.Scripts.External.UserData;
using App.Scripts.General.UserData.Levels.Data;

namespace App.Scripts.General.UserData.Levels
{
    public class LevelProgressDataService
    {
        private readonly IUserDataContainer _userDataContainer;

        public LevelProgressDataService(IUserDataContainer userDataContainer)
        {
            _userDataContainer = userDataContainer;
        }
        
        public void PassLevel(int packIndex, int levelIndex)
        {
            LevelPackProgressDictionary levelPackDictionary =
                (LevelPackProgressDictionary) _userDataContainer.GetData<LevelPackProgressDictionary>();
            
            if (!levelPackDictionary.ContainsKey(packIndex))
            {
                levelPackDictionary.Add(packIndex, new());
            }

            if (levelPackDictionary[packIndex].PassedLevels - 1 >= levelIndex)
                return;

            levelPackDictionary[packIndex].PassedLevels++;
            
            _userDataContainer.SaveData<LevelPackProgressDictionary>();
        }

        public int GetPassedLevelsForPackIndex(int packIndex)
        {
            LevelPackProgressDictionary levelPackDictionary =
                (LevelPackProgressDictionary) _userDataContainer.GetData<LevelPackProgressDictionary>();
            
            if (!levelPackDictionary.ContainsKey(packIndex))
            {
                return 0;
            }

            return levelPackDictionary[packIndex].PassedLevels;
        }
    }
}