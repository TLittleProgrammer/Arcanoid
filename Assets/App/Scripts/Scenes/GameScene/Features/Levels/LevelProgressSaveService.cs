using System.Collections.Generic;
using App.Scripts.External.UserData;
using App.Scripts.Scenes.GameScene.Features.Levels.SavedLevelProgress;

namespace App.Scripts.Scenes.GameScene.Features.Levels
{
    public class LevelProgressSaveService
    {
        private readonly List<ILevelProgressSavable> _levelProgressSavable;
        private readonly IUserDataContainer _userDataContainer;

        public LevelProgressSaveService(List<ILevelProgressSavable> levelProgressSavable, IUserDataContainer userDataContainer)
        {
            _levelProgressSavable = levelProgressSavable;
            _userDataContainer = userDataContainer;
        }

        public void SaveProgress()
        {
            LevelDataProgress levelDataProgress = new LevelDataProgress();

            foreach (ILevelProgressSavable progressSavable in _levelProgressSavable)
            {
                progressSavable.SaveProgress(levelDataProgress);
            }
            
            _userDataContainer.FastSaveData(levelDataProgress);
        }
    }
}