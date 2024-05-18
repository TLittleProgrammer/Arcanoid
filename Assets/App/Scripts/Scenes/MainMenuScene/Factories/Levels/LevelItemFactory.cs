using System.Linq;
using App.Scripts.External.Components;
using App.Scripts.External.GameStateMachine;
using App.Scripts.External.Localisation;
using App.Scripts.General.Constants;
using App.Scripts.General.Levels;
using App.Scripts.General.States;
using App.Scripts.General.UserData.Data;
using App.Scripts.Scenes.MainMenuScene.Constants;
using App.Scripts.Scenes.MainMenuScene.LevelPacks;
using App.Scripts.Scenes.MainMenuScene.LevelPacks.Configs;
using TMPro;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.MainMenuScene.Factories.Levels
{
    public class LevelItemFactory : IFactory<int, LevelPack, ILevelItemView>
    {
        private readonly ILevelItemView _prefab;
        private readonly ITransformable _prefabParent;
        private readonly LevelPackProgressDictionary _levelPackProgressDictionary;
        private readonly LevelItemViewByTypeProvider _levelItemViewByTypeProvider;
        private readonly ILevelPackTransferData _levelPackTransferData;
        private readonly IStateMachine _stateMachine;
        private readonly DiContainer _diContainer;

        public LevelItemFactory(
            ILevelItemView prefab,
            ITransformable prefabParent,
            LevelPackProgressDictionary levelPackProgressDictionary,
            LevelItemViewByTypeProvider levelItemViewByTypeProvider,
            ILevelPackTransferData levelPackTransferData,
            IStateMachine stateMachine,
            DiContainer diContainer)
        {
            _prefab = prefab;
            _prefabParent = prefabParent;
            _levelPackProgressDictionary = levelPackProgressDictionary;
            _levelItemViewByTypeProvider = levelItemViewByTypeProvider;
            _levelPackTransferData = levelPackTransferData;
            _stateMachine = stateMachine;
            _diContainer = diContainer;
        }
        
        public ILevelItemView Create(int packIndex, LevelPack levelPack)
        {
            ILevelItemView levelItemView = _diContainer.InstantiatePrefab(_prefab.GameObject, _prefabParent.Transform).GetComponent<ILevelItemView>();
            VisualTypeId visualType = GetVisualType(packIndex, levelPack);
            
            ChangeVisual(packIndex, levelItemView, levelPack, visualType);
            SubsribeOnClickIfNeed(levelItemView, packIndex, levelPack, visualType);
            
            return levelItemView;
        }

        private void SubsribeOnClickIfNeed(ILevelItemView levelItemView, int packIndex, LevelPack levelPack, VisualTypeId visualType)
        {
            if (visualType is not VisualTypeId.NotOpened)
            {
                levelItemView.Clicked += () =>
                {
                    _levelPackTransferData.NeedLoadLevel = true;
                    _levelPackTransferData.LevelIndex = _levelPackProgressDictionary.ContainsKey(packIndex) ? _levelPackProgressDictionary[packIndex].PassedLevels : 0;
                    _levelPackTransferData.LevelPack = levelPack;
                    _levelPackTransferData.LevelIndex = packIndex;
                    _levelPackTransferData.LevelPackProgress = 0f;
                    
                    _stateMachine.Enter<LoadingSceneState, string, bool>(SceneNaming.Game, false);
                };
            }
        }

        private void ChangeVisual(int packIndex, ILevelItemView levelItemView, LevelPack levelPack, VisualTypeId visualType)
        {
            LevelItemViewData levelItemViewData = _levelItemViewByTypeProvider.Views[visualType];
            int passedLevels = _levelPackProgressDictionary.ContainsKey(packIndex) ? _levelPackProgressDictionary[packIndex].PassedLevels : 0;
                
            levelItemView.UpdateVisual(new()
            {
                LevelViewData = levelItemViewData,
                LevelPack = levelPack,
                VisualTypeId = visualType,
                PassedLevels = passedLevels,
                LocaleKey = visualType is VisualTypeId.NotOpened ? MainSceneLocaleConstants.GalacticNotFoundKey : levelPack.LocaleKey
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

            if (packIndex == lastOpenedPack.Key + 1 &&
                lastOpenedPack.Value.PassedLevels >= levelPack.Levels.Count)
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