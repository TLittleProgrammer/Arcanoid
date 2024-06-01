using System.Collections.Generic;
using App.Scripts.External.UserData;
using App.Scripts.General.LevelPackInfoService;
using App.Scripts.Scenes.GameScene.Features.Levels.SavedLevelProgress;

namespace App.Scripts.Scenes.GameScene.Features.Levels
{
    public class LevelProgressSaveService
    {
        private readonly List<ILevelProgressSavable> _levelProgressSavable;
        private readonly IDataProvider<LevelDataProgress> _levelDataProgressProvider;
        private readonly ILevelPackInfoService _levelPackInfoService;

        public LevelProgressSaveService(
            List<ILevelProgressSavable> levelProgressSavable,
            IDataProvider<LevelDataProgress> levelDataProgressProvider,
            ILevelPackInfoService levelPackInfoService)
        {
            _levelProgressSavable = levelProgressSavable;
            _levelDataProgressProvider = levelDataProgressProvider;
            _levelPackInfoService = levelPackInfoService;
        }

        public void SaveProgress()
        {
            LevelDataProgress levelDataProgress = new LevelDataProgress();

            foreach (ILevelProgressSavable progressSavable in _levelProgressSavable)
            {
                progressSavable.SaveProgress(levelDataProgress);
            }

            _levelDataProgressProvider.SaveData(levelDataProgress);
        }
    }
}