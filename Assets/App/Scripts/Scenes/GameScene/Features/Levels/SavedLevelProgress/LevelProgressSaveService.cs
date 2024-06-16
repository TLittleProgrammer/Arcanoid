using System.Collections.Generic;
using App.Scripts.External.UserData;

namespace App.Scripts.Scenes.GameScene.Features.Levels.SavedLevelProgress
{
    public class LevelProgressSaveService : ILevelProgressSaveService
    {
        private readonly List<ILevelProgressSavable> _levelProgressSavable;
        private readonly IDataProvider<LevelDataProgress> _levelDataProgressProvider;

        public LevelProgressSaveService(List<ILevelProgressSavable> levelProgressSavable, IDataProvider<LevelDataProgress> levelDataProgressProvider)
        {
            _levelProgressSavable = levelProgressSavable;
            _levelDataProgressProvider = levelDataProgressProvider;
        }

        public void SaveProgress()
        {
            LevelDataProgress levelDataProgress = new LevelDataProgress();

            foreach (ILevelProgressSavable progressSavable in _levelProgressSavable)
            {
                progressSavable.SaveProgress(levelDataProgress);
            }

            _levelDataProgressProvider.SaveData();
        }
    }
}