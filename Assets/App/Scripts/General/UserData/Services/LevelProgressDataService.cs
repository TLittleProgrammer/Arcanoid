using App.Scripts.General.UserData.Data;

namespace App.Scripts.General.UserData.Services
{
    public class LevelProgressDataService
    {
        private readonly IUserDataContainer _userDataContainer;

        public LevelProgressDataService(IUserDataContainer userDataContainer)
        {
            _userDataContainer = userDataContainer;
        }
        
        public void PassLevel(int packIndex)
        {
            LevelPackProgressDictionary levelPackDictionary =
                (LevelPackProgressDictionary) _userDataContainer.GetData<LevelPackProgressDictionary>();
            
            if (!levelPackDictionary.ContainsKey(packIndex))
            {
                levelPackDictionary.Add(packIndex, new());
            }

            levelPackDictionary[packIndex].PassedLevels++;
        }
    }
}