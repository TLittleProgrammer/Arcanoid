using App.Scripts.General.LevelPackInfoService;
using App.Scripts.Scenes.GameScene.Features.Levels.General;
using App.Scripts.Scenes.GameScene.Features.Levels.SavedLevelProgress;
using Newtonsoft.Json;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Levels.Loading
{
    public sealed class LevelDataChooser : ILevelDataChooser, ILevelProgressSavable
    {
        private readonly ILevelPackInfoService _levelPackInfoService;
        private readonly TextAsset _choosedFromSceneLevelData;

        public LevelDataChooser(
            ILevelPackInfoService levelPackInfoService,
            TextAsset choosedFromSceneLevelData)
        {
            _levelPackInfoService = levelPackInfoService;
            _choosedFromSceneLevelData = choosedFromSceneLevelData;
        }
        
        public LevelData GetLevelData()
        {
            var data = _levelPackInfoService.GetData();
            if (data is not null && data.NeedLoadLevel)
            {
                return JsonConvert.DeserializeObject<LevelData>(data.LevelPack.Levels[data.LevelIndex].text);
            }

            return JsonConvert.DeserializeObject<LevelData>(_choosedFromSceneLevelData.text);
        }

        public LevelData GetNextLevelData()
        {
            var data = _levelPackInfoService.UpdateLevelPackTransferData();
            return JsonConvert.DeserializeObject<LevelData>(data.LevelPack.Levels[data.LevelIndex].text);
        }

        public void SaveProgress(LevelDataProgress levelDataProgress)
        {
            levelDataProgress.LevelData = GetLevelData();
        }
    }
}