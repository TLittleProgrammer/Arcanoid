using App.Scripts.External.UserData;
using App.Scripts.General.UserData.Levels.Data;

namespace App.Scripts.General.UserData.Levels
{
    public class LevelPackProgressDataService
    {
        private readonly IDataProvider<LevelPackProgressDictionary> _levelPackProgressProvider;

        public LevelPackProgressDataService(IDataProvider<LevelPackProgressDictionary> levelPackProgressProvider)
        {
            _levelPackProgressProvider = levelPackProgressProvider;
        }
        
        public void PassLevel(int packIndex, int levelIndex)
        {
            LevelPackProgressDictionary levelPackDictionary = _levelPackProgressProvider.GetData();
            
            if (!levelPackDictionary.ContainsKey(packIndex))
            {
                levelPackDictionary.Add(packIndex, new());
            }

            if (levelPackDictionary[packIndex].PassedLevels - 1 >= levelIndex)
                return;

            levelPackDictionary[packIndex].PassedLevels++;
            
            _levelPackProgressProvider.SaveData();
        }

        public int GetPassedLevelsForPackIndex(int packIndex)
        {
            LevelPackProgressDictionary levelPackDictionary = _levelPackProgressProvider.GetData();
            
            if (!levelPackDictionary.ContainsKey(packIndex))
            {
                return 0;
            }

            return levelPackDictionary[packIndex].PassedLevels;
        }
    }
}