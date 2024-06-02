using App.Scripts.General.Levels;
using App.Scripts.General.Levels.LevelPackInfoService;
using App.Scripts.Scenes.GameScene.Features.Levels.General;
using App.Scripts.Scenes.GameScene.Features.Levels.SavedLevelProgress;
using App.Scripts.Scenes.GameScene.Features.Levels.SavedLevelProgress.Data;
using Newtonsoft.Json;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Levels.Loading
{
    public sealed class LevelDataChooser : ILevelDataChooser, ILevelProgressSavable, IInitializeByLevelProgress
    {
        private readonly ILevelPackInfoService _levelPackInfoService;
        private readonly TextAsset _choosedFromSceneLevelData;

        public LevelDataChooser(ILevelPackInfoService levelPackInfoService, TextAsset choosedFromSceneLevelData)
        {
            _levelPackInfoService = levelPackInfoService;
            _choosedFromSceneLevelData = choosedFromSceneLevelData;
        }

        public LevelData GetLevelData()
        {
            var data = _levelPackInfoService.LevelPackTransferData;
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

            SavePackData(levelDataProgress);
        }

        private void SavePackData(LevelDataProgress levelDataProgress)
        {
            var levelData = _levelPackInfoService.LevelPackTransferData;
            LevelTransferPackData data = new();
            
            if (levelData is null)
            {
                levelDataProgress.LevelTransferPackData = null;
                return;
            }

            LevelPackData levelPackData = new();

            levelPackData.Levels = new();
            levelPackData.EnergyPrice = levelData.LevelPack.EnergyPrice;
            levelPackData.EnergyAddForWin = levelData.LevelPack.EnergyAddForWin;
            levelPackData.LocaleKey = levelData.LevelPack.LocaleKey;
            levelPackData.GalacticBackground = levelData.LevelPack.GalacticBackgroundKey;
            levelPackData.GalacticIcon = levelData.LevelPack.GalacticIconKey;

            foreach (TextAsset asset in levelData.LevelPack.Levels)
            {
                levelPackData.Levels.Add(asset.text);
            }

            data.LevelIndex = levelData.LevelIndex;
            data.PackIndex = levelData.PackIndex;
            data.NeedContinue = levelData.NeedContinue;
            data.NeedLoadLevel = levelData.NeedLoadLevel;
            data.LevelPack = levelPackData;

            levelDataProgress.LevelTransferPackData = data;
        }

        public void LoadProgress(LevelDataProgress levelDataProgress)
        {
            ILevelPackTransferData levelPackTransferData = new LevelPackTransferData();
            LevelTransferPackData savedData = levelDataProgress.LevelTransferPackData;

            levelPackTransferData.NeedContinue = savedData.NeedContinue;
            levelPackTransferData.NeedLoadLevel = savedData.NeedLoadLevel;
            levelPackTransferData.LevelIndex = savedData.LevelIndex;
            levelPackTransferData.PackIndex = savedData.PackIndex;

            LevelPack levelPackData = ScriptableObject.CreateInstance<LevelPack>();

            levelPackData.Levels = new();
            levelPackData.EnergyPrice = savedData.LevelPack.EnergyPrice;
            levelPackData.EnergyAddForWin = savedData.LevelPack.EnergyAddForWin;
            levelPackData.LocaleKey = savedData.LevelPack.LocaleKey;
            levelPackData.GalacticIconKey = savedData.LevelPack.GalacticBackground;
            levelPackData.GalacticBackgroundKey = savedData.LevelPack.GalacticBackground;


            foreach (string str in savedData.LevelPack.Levels)
            {
                TextAsset textAsset = new(str);
                
                levelPackData.Levels.Add(textAsset);
            }
            
            levelPackTransferData.LevelPack = levelPackData;
            
            _levelPackInfoService.LevelPackTransferData = levelPackTransferData;
        }
    }
}