using App.Scripts.External.GameStateMachine;
using App.Scripts.General.Components;
using App.Scripts.General.Levels;
using App.Scripts.General.States;
using App.Scripts.General.UserData.Data;
using App.Scripts.Scenes.GameScene.Components;
using App.Scripts.Scenes.MainMenuScene.LevelPacks;
using App.Scripts.Scenes.MainMenuScene.LevelPacks.Configs;
using TMPro;
using UnityEngine;
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
        private readonly ILevelPackTransferData _levelPackTransferData;
        private readonly IStateMachine _stateMachine;

        public LevelItemFactory(
            ILevelItemView prefab,
            ITransformable prefabParent,
            DiContainer diContainer,
            LevelPackProgressDictionary levelPackProgressDictionary,
            LevelItemViewByTypeProvider levelItemViewByTypeProvider,
            ILevelPackTransferData levelPackTransferData,
            IStateMachine stateMachine)
        {
            _prefab = prefab;
            _prefabParent = prefabParent;
            _diContainer = diContainer;
            _levelPackProgressDictionary = levelPackProgressDictionary;
            _levelItemViewByTypeProvider = levelItemViewByTypeProvider;
            _levelPackTransferData = levelPackTransferData;
            _stateMachine = stateMachine;
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
                levelItemView.GalacticPassedLevels.text = $"0/{levelPack.Levels.Count}";
                levelItemView.GalacticIcon.gameObject.SetActive(false);
                levelItemView.LockIcon.gameObject.SetActive(true);

                levelItemView.GalacticName.Text.colorGradient = new VertexGradient(Color.white, Color.white, Color.white, Color.white);
                levelItemView.GalacticText.colorGradient = new VertexGradient(Color.white, Color.white, Color.white, Color.white);
                levelItemView.GalacticName.Text.color = levelItemViewData.InactiveColorForNaming;
                levelItemView.GalacticText.color = levelItemViewData.InactiveColorForNaming;
                levelItemView.GalacticPassedLevels.color = levelItemViewData.PassedLevelsColor;
            }
            else
            {
                levelItemView.GalacticIcon.sprite = levelPack.GalacticIcon;
                levelItemView.GalacticPassedLevels.text = $"{_levelPackProgressDictionary[packIndex].PassedLevels}/{levelPack.Levels.Count}";
                levelItemView.GalacticIcon.gameObject.SetActive(true);
                levelItemView.LockIcon.gameObject.SetActive(false);

                levelItemView.Clicked += () =>
                {
                    _levelPackTransferData.NeedLoadLevel = true;
                    _levelPackTransferData.LevelIndex = _levelPackProgressDictionary[packIndex].PassedLevels;
                    _levelPackTransferData.LevelPack = levelPack;
                    _levelPackTransferData.LevelIndex = packIndex;
                    _levelPackTransferData.LevelPackProgress = 0f;
                    
                    _stateMachine.Enter<LoadingSceneState, string, bool>("2.Game", false);
                };
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