using App.Scripts.General.Components;
using App.Scripts.General.UserData.Data;
using App.Scripts.General.UserData.Services;
using App.Scripts.Scenes.MainMenuScene.LevelPacks;
using App.Scripts.Scenes.MainMenuScene.LevelPacks.Configs;
using Zenject;

namespace App.Scripts.Scenes.MainMenuScene.Factories.Levels
{
    public class LevelItemFactory
    {
        private readonly ILevelItemView _prefab;
        private readonly ITransformable _prefabParent;
        private readonly DiContainer _diContainer;
        private readonly LevelPackProgressDictionary _levelPackProgressDictionary;
        private readonly LevelItemViewByTypeProvider _levelItemViewByTypeProvider;

        public LevelItemFactory(
            ILevelItemView prefab,
            ITransformable prefabParent,
            DiContainer diContainer,
            LevelPackProgressDictionary levelPackProgressDictionary,
            LevelItemViewByTypeProvider levelItemViewByTypeProvider)
        {
            _prefab = prefab;
            _prefabParent = prefabParent;
            _diContainer = diContainer;
            _levelPackProgressDictionary = levelPackProgressDictionary;
            _levelItemViewByTypeProvider = levelItemViewByTypeProvider;
        }
        
        public ILevelItemView Create(int packIndex, LevelPack levelPack)
        {
            ILevelItemView levelItemView = _diContainer.InstantiatePrefabForComponent<LevelItemView>(_prefab.GameObject, _prefabParent.Transform);

            ChangeVisual(packIndex, levelItemView, levelPack);
            
            
            return levelItemView;
        }

        private void ChangeVisual(int packIndex, ILevelItemView levelItemView, LevelPack levelPack)
        {
            LevelItemTypeId visualType = GetVisualType(packIndex, levelPack);

            LevelItemViewData levelItemViewData = _levelItemViewByTypeProvider.Views[visualType];

            levelItemView.Glow.sprite = levelItemViewData.Glow;
            levelItemView.BigBackground.sprite = levelItemViewData.BigBackground;
            levelItemView.MaskableImage.sprite = levelItemViewData.MaskableImage;
            levelItemView.LeftImageHalf.sprite = levelItemViewData.LeftImageHalf;
            levelItemView.GalacticPassedLevelsBackground.sprite = levelItemViewData.GalacticPassedLevelsBackground;
            levelItemView.GalacticName.SetToken(levelPack.LocaleKey);
            
            if (visualType is LevelItemTypeId.NotOpened)
            {
                levelItemView.GalacticIcon.sprite = levelItemViewData.GalacticIcon;
                levelItemView.GalacticPassedLevels.text = $"0/{levelPack.Levels.Count}";
            }
            else
            {
                levelItemView.GalacticIcon.sprite = levelPack.GalacticIcon;
                levelItemView.GalacticPassedLevels.text = $"{_levelPackProgressDictionary[packIndex].PassedLevels}/{levelPack.Levels.Count}";
            }
        }

        private LevelItemTypeId GetVisualType(int packIndex, LevelPack levelPack)
        {
            if (packIndex == 0 && _levelPackProgressDictionary.Count == 0)
            {
                _levelPackProgressDictionary.Add(0, new());
                return LevelItemTypeId.InProgress;
            }
            
            if (!_levelPackProgressDictionary.ContainsKey(packIndex))
            {
                return LevelItemTypeId.NotOpened;
            }

            if (_levelPackProgressDictionary[packIndex].PassedLevels == levelPack.Levels.Count)
            {
                return LevelItemTypeId.Passed;
            }

            return LevelItemTypeId.InProgress;
        }
    }
}