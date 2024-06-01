using System.Collections.Generic;
using System.Linq;
using App.Scripts.External.Components;
using App.Scripts.External.UserData;
using App.Scripts.General.Levels;
using App.Scripts.General.UserData.Levels.Data;
using App.Scripts.Scenes.MainMenuScene.Command;
using App.Scripts.Scenes.MainMenuScene.Constants;
using App.Scripts.Scenes.MainMenuScene.LevelPacks.Configs;

namespace App.Scripts.Scenes.MainMenuScene.LevelPacks.MonoBehaviours
{
    public class LevelPackViewModel
    {
        private readonly LevelPackModel _levelPackModel;
        private readonly LevelItemViewByTypeProvider _levelItemViewByTypeProvider;
        private readonly ILoadLevelCommand _loadLevelCommand;
        private readonly LevelPackProgressDictionary _levelPackProgressDictionary;

        public LevelPackViewModel(
            LevelPackModel levelPackModel,
            IDataProvider<LevelPackProgressDictionary> levelPackProvider,
            LevelItemViewByTypeProvider levelItemViewByTypeProvider,
            ILoadLevelCommand loadLevelCommand)
        {
            _levelPackModel = levelPackModel;
            _levelItemViewByTypeProvider = levelItemViewByTypeProvider;
            _loadLevelCommand = loadLevelCommand;
            _levelPackProgressDictionary = levelPackProvider.GetData();
        }

        public void CreateAllLeveViewPacks(ITransformable levelPackContainerView)
        {
            List<LevelItemData> levelItemDatas = _levelPackModel.GetLevelItemDatas();

            foreach (LevelItemData levelItemData in levelItemDatas)
            {
                levelItemData.LevelItemView.GameObject.transform.SetParent(levelPackContainerView.Transform, false);
                
                VisualTypeId visualType = GetVisualType(levelItemData.PackIndex, levelItemData.LevelPack);
            
                ChangeVisual(levelItemData, visualType);
                SubsribeOnClickIfNeed(levelItemData, visualType);
            }
        }
        
        private void SubsribeOnClickIfNeed(LevelItemData levelItemData, VisualTypeId visualType)
        {
            int targetLevelIndex;
            if (visualType is not VisualTypeId.NotOpened)
            {
                if (visualType is VisualTypeId.Passed)
                {
                    targetLevelIndex = 0;
                }
                else
                {
                    targetLevelIndex = _levelPackProgressDictionary.ContainsKey(levelItemData.PackIndex)
                        ? _levelPackProgressDictionary[levelItemData.PackIndex].PassedLevels
                        : 0;
                }
                
                levelItemData.LevelItemView.Clicked += () =>
                {
                    _loadLevelCommand.Execute(levelItemData, targetLevelIndex);
                };
            }
        }
        
        private void ChangeVisual(LevelItemData levelItemData, VisualTypeId visualType)
        {
            LevelItemViewData levelItemViewData = _levelItemViewByTypeProvider.Views[visualType];
            int passedLevels = _levelPackProgressDictionary.ContainsKey(levelItemData.PackIndex) ? _levelPackProgressDictionary[levelItemData.PackIndex].PassedLevels : 0;
                
            levelItemData.LevelItemView.UpdateVisual(new()
            {
                LevelViewData = levelItemViewData,
                LevelPack = levelItemData.LevelPack,
                VisualTypeId = visualType,
                PassedLevels = passedLevels,
                LocaleKey = visualType is VisualTypeId.NotOpened ? MainSceneLocaleConstants.GalacticNotFoundKey : levelItemData.LevelPack.LocaleKey
            });
        }

        private VisualTypeId GetVisualType(int packIndex, LevelPack levelPack)
        {
            if (packIndex == 0 && _levelPackProgressDictionary.Count == 0)
            {
                _levelPackProgressDictionary.Add(0, new());
                return VisualTypeId.InProgress;
            }

            var lastOpenedPack = _levelPackProgressDictionary.Last();

            if (packIndex == lastOpenedPack.Key + 1 && lastOpenedPack.Value.PassedLevels >= levelPack.Levels.Count)
            {
                return VisualTypeId.InProgress;
            }
            
            if (!_levelPackProgressDictionary.ContainsKey(packIndex))
            {
                return VisualTypeId.NotOpened;
            }

            if (_levelPackProgressDictionary[packIndex].PassedLevels >= levelPack.Levels.Count)
            {
                return VisualTypeId.Passed;
            }

            return VisualTypeId.InProgress;
        }
    }
}