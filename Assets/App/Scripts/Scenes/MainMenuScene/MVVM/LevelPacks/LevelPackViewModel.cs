using System.Collections.Generic;
using System.Linq;
using App.Scripts.External.Components;
using App.Scripts.External.UserData;
using App.Scripts.General.Levels;
using App.Scripts.General.Providers;
using App.Scripts.General.UserData.Levels.Data;
using App.Scripts.Scenes.MainMenuScene.Command;
using App.Scripts.Scenes.MainMenuScene.Constants;
using App.Scripts.Scenes.MainMenuScene.LevelPacks.Configs;
using TMPro;
using UnityEngine;

namespace App.Scripts.Scenes.MainMenuScene.LevelPacks.MonoBehaviours
{
    public class LevelPackViewModel
    {
        private readonly LevelPackModel _levelPackModel;
        private readonly LevelItemViewByTypeProvider _levelItemViewByTypeProvider;
        private readonly ILoadLevelCommand _loadLevelCommand;
        private readonly SpriteProvider _spriteProvider;
        private readonly LevelPackProgressDictionary _levelPackProgressDictionary;

        public LevelPackViewModel(
            LevelPackModel levelPackModel,
            IDataProvider<LevelPackProgressDictionary> levelPackProvider,
            LevelItemViewByTypeProvider levelItemViewByTypeProvider,
            ILoadLevelCommand loadLevelCommand,
            SpriteProvider spriteProvider)
        {
            _levelPackModel = levelPackModel;
            _levelItemViewByTypeProvider = levelItemViewByTypeProvider;
            _loadLevelCommand = loadLevelCommand;
            _spriteProvider = spriteProvider;
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

            ILevelItemView item = levelItemData.LevelItemView;

            UpdateGeneralView(item, levelItemViewData, levelItemData.LevelPack, visualType);
            UpdateVisualByType(item, levelItemViewData, visualType, levelItemData.LevelPack, passedLevels);
        }

        private void UpdateVisualByType(ILevelItemView item, LevelItemViewData levelItemViewData, VisualTypeId visualType, LevelPack levelPack, int passedLevels)
        {
            bool isOpened = visualType is not VisualTypeId.NotOpened;
            
            item.GalacticIcon.gameObject.SetActive(isOpened);
            item.EnergyPanel.gameObject.SetActive(isOpened);
            item.LockIcon.gameObject.SetActive(!isOpened);

            if (isOpened)
            {
                item.EnergyText.text = levelPack.EnergyPrice.ToString();
                item.GalacticIcon.sprite = _spriteProvider.Sprites[levelPack.GalacticIconKey];
                item.GalacticPassedLevels.text = $"{passedLevels}/{levelPack.Levels.Count}";
            }
            else
            {
                item.GalacticPassedLevels.text = $"0/{levelPack.Levels.Count}";
                
                item.GalacticName.Text.colorGradient = new VertexGradient(Color.white, Color.white, Color.white, Color.white);
                item.GalacticText.colorGradient      = new VertexGradient(Color.white, Color.white, Color.white, Color.white);
                item.GalacticName.Text.color         = levelItemViewData.InactiveColorForNaming;
                item.GalacticText.color              = levelItemViewData.InactiveColorForNaming;
                item.GalacticPassedLevels.color      = levelItemViewData.PassedLevelsColor;
            }
        }

        private void UpdateGeneralView(ILevelItemView item, LevelItemViewData levelViewData, LevelPack levelPack, VisualTypeId visualType)
        {
            item.Glow.sprite = levelViewData.Glow;
            item.BigBackground.sprite = levelViewData.BigBackground;
            item.MaskableImage.sprite = levelViewData.MaskableImage;
            item.LeftImageHalf.sprite = levelViewData.LeftImageHalf;
            item.GalacticPassedLevelsBackground.sprite = levelViewData.GalacticPassedLevelsBackground;
            
            item.GalacticName.SetToken(levelPack.LocaleKey);
            item.SubTextLocale.SetToken(visualType is VisualTypeId.NotOpened
                ? MainSceneLocaleConstants.GalacticNotFoundKey
                : levelPack.LocaleKey);
        }

        private VisualTypeId GetVisualType(int packIndex, LevelPack levelPack)
        {
            if (packIndex == 0 && _levelPackProgressDictionary.Count == 0)
            {
                _levelPackProgressDictionary.Add(0, new());
                return VisualTypeId.InProgress;
            }

            var lastOpenedPack = _levelPackProgressDictionary.Last();

            if (packIndex == lastOpenedPack.Key && lastOpenedPack.Value.PassedLevels >= levelPack.Levels.Count)
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